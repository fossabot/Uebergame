//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// The TSShapeConstructor object allows you to apply a set of transformations
// to a 3space shape after it is loaded by Torque, but _before_ the shape is used
// by any other object (eg. Player, StaticShape etc). The sort of transformations
// available include adding, renaming and removing nodes and sequences. This GUI
// is a visual wrapper around TSShapeConstructor which allows you to build up the
// transformation set without having to get your hands dirty with TorqueScript.
//
// Removing a node, sequence, mesh or detail poses a problem. These operations
// permanently delete a potentially large amount of data scattered throughout
// the shape, and there is no easy way to restore it if the user 'undoes' the
// delete. Although it is possible to store the deleted data somewhere and restore
// it on undo, it is not easy to get right, and ugly as hell to implement. For
// example, removing a node would require storing the node name, the
// translation/rotation/scale matters bit for each sequence, all node transform
// keyframes, the IDs of any objects that were attached to the node, skin weights
// etc, then restoring all that data into the original place on undo. Frankly,
// TSShape was never designed to be modified dynamically like that.
//
// So......currently we wimp out completely and just don't support undo for those
// remove operations. Lame, I know, but the best I can do for now.
//
// This file implements all of the actions that can be applied by the GUI. Each
// action has 3 methods:
//
//    doit: called the first time the action is performed
//    undo: called to undo the action
//    redo: called to redo the action (usually the same as doit)
//
// In each case, the appropriate change is made to the shape, and the GUI updated.
//
// TSShapeConstructor keeps track of all the changes made and provides a simple
// way to save the modifications back out to a script file.

// The VehicleEditor uses its own UndoManager
if ( !isObject( VehicleEdUndoManager ) )
    new UndoManager( VehicleEdUndoManager );

function VehicleEdUndoManager::updateUndoMenu( %this, %editMenu ) {
   Lab.updateUndoMenu();   
}

//------------------------------------------------------------------------------
// Helper functions for creating and applying GUI operations

function VehicleEditor::createAction( %this, %class, %desc ) {
    pushInstantGroup();
    %action = new UndoScriptAction() {
        class = %class;
        superClass = BaseVehicleEdAction;
        actionName = %desc;
        done = 0;
    };
    popInstantGroup();
    return %action;
}

function VehicleEditor::doAction( %this, %action ) {
    if ( %action.doit() ) {
        VehicleEditor.setDirty( true );
        %action.addToManager( VehicleEdUndoManager );
    } else {
        LabMsgOK( "Error", %action.actionName SPC "failed. Check the console for error messages.", "" );
    }
}

function BaseVehicleEdAction::redo( %this ) {
    // Default redo action is the same as the doit action
    if ( %this.doit() ) {
        VehicleEditor.setDirty( true );
    } else {
        LabMsgOK( "Error", "Redo" SPC %this.actionName SPC "failed. Check the console for error messages.", "" );
    }
}

function BaseVehicleEdAction::undo( %this ) {
    VehicleEditor.setDirty( true );
}

//------------------------------------------------------------------------------

function VehicleEditor::doRemoveShapeData( %this, %type, %name ) {
    // Removing data from the shape cannot be undone => so warn the user first
    LabMsgYesNo( "Warning", "Deleting a " @ %type @ " cannot be undone. Do " @
                      "you want to continue?", "VehicleEditor.doRemove" @ %type @ "( \"" @ %name @ "\" );", "" );
}

//------------------------------------------------------------------------------
// Add node
function VehicleEditor::doAddNode( %this, %nodeName, %parentName, %transform ) {
    %action = %this.createAction( ActionAddNode, "Add node" );
    %action.nodeName = %nodeName;
    %action.parentName = %parentName;
    %action.transform = %transform;

    %this.doAction( %action );
}

function ActionAddNode::doit( %this ) {
    if ( VehicleEditor.shape.addNode( %this.nodeName, %this.parentName, %this.transform ) ) {
        VehicleEdPropWindow.update_onNodeAdded( %this.nodeName, -1 );
        return true;
    }
    return false;
}

function ActionAddNode::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.removeNode( %this.nodeName ) )
        VehicleEdPropWindow.update_onNodeRemoved( %this.nodeName, 1 );
}

//------------------------------------------------------------------------------
// Remove node
function VehicleEditor::doRemoveNode( %this, %nodeName ) {
    %action = %this.createAction( ActionRemoveNode, "Remove node" );
    %action.nodeName =%nodeName;
    %action.nodeChildIndex = VehicleEdNodeTreeView.getChildIndexByName( %nodeName );

    // Need to delete all child nodes of this node as well, so recursively collect
    // all of the names.
    %action.nameList = %this.getNodeNames( %nodeName, "" );
    %action.nameCount = getFieldCount( %action.nameList );
    for ( %i = 0; %i < %action.nameCount; %i++ )
        %action.names[%i] = getField( %action.nameList, %i );

    %this.doAction( %action );
}

function ActionRemoveNode::doit( %this ) {
    for ( %i = 0; %i < %this.nameCount; %i++ )
        VehicleEditor.shape.removeNode( %this.names[%i] );

    // Update GUI
    VehicleEdPropWindow.update_onNodeRemoved( %this.nameList, %this.nameCount );

    return true;
}

function ActionRemoveNode::undo( %this ) {
    Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Rename node
function VehicleEditor::doRenameNode( %this, %oldName, %newName ) {
    %action = %this.createAction( ActionRenameNode, "Rename node" );
    %action.oldName = %oldName;
    %action.newName = %newName;

    %this.doAction( %action );
}

function ActionRenameNode::doit( %this ) {
    if ( VehicleEditor.shape.renameNode( %this.oldName, %this.newName ) ) {
        VehicleEdPropWindow.update_onNodeRenamed( %this.oldName, %this.newName );
        return true;
    }
    return false;
}

function ActionRenameNode::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.renameNode( %this.newName, %this.oldName ) )
        VehicleEdPropWindow.update_onNodeRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Set node parent
function VehicleEditor::doSetNodeParent( %this, %name, %parent ) {
    if ( %parent $= "<root>" )
        %parent = "";

    %action = %this.createAction( ActionSetNodeParent, "Set parent node" );
    %action.nodeName = %name;
    %action.parentName = %parent;
    %action.oldParentName = VehicleEditor.shape.getNodeParentName( %name );

    %this.doAction( %action );
}

function ActionSetNodeParent::doit( %this ) {
    if ( VehicleEditor.shape.setNodeParent( %this.nodeName, %this.parentName ) ) {
        VehicleEdPropWindow.update_onNodeParentChanged( %this.nodeName );
        return true;
    }
    return false;
}

function ActionSetNodeParent::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.setNodeParent( %this.nodeName, %this.oldParentName ) )
        VehicleEdPropWindow.update_onNodeParentChanged( %this.nodeName );
}

//------------------------------------------------------------------------------
// Edit node transform
function VehicleEditor::doEditNodeTransform( %this, %nodeName, %newTransform, %isWorld, %gizmoID ) {
    // If dragging the 3D gizmo, combine all movement into a single action. Undoing
    // that action will return the node to where it was when the gizmo drag started.
    %last = VehicleEdUndoManager.getUndoAction( VehicleEdUndoManager.getUndoCount() - 1 );
    if ( ( %last != -1 ) && ( %last.class $= ActionEditNodeTransform ) &&
            ( %last.nodeName $= %nodeName ) && ( %last.gizmoID != -1 ) && ( %last.gizmoID == %gizmoID ) ) {
        // Use the last action to do the edit, and modify it so it only applies
        // the latest transform
        %last.newTransform = %newTransform;
        %last.isWorld = %isWorld;
        %last.doit();
        VehicleEditor.setDirty( true );
    } else {
        %action = %this.createAction( ActionEditNodeTransform, "Edit node transform" );
        %action.nodeName = %nodeName;
        %action.newTransform = %newTransform;
        %action.isWorld = %isWorld;
        %action.gizmoID = %gizmoID;
        %action.oldTransform = %this.shape.getNodeTransform( %nodeName, %isWorld );

        %this.doAction( %action );
    }
}

function ActionEditNodeTransform::doit( %this ) {
    VehicleEditor.shape.setNodeTransform( %this.nodeName, %this.newTransform, %this.isWorld );
    VehicleEdPropWindow.update_onNodeTransformChanged();
    return true;
}

function ActionEditNodeTransform::undo( %this ) {
    Parent::undo( %this );

    VehicleEditor.shape.setNodeTransform( %this.nodeName, %this.oldTransform, %this.isWorld );
    VehicleEdPropWindow.update_onNodeTransformChanged();
}

//------------------------------------------------------------------------------
// Add sequence
function VehicleEditor::doAddSequence( %this, %seqName, %from, %start, %end ) {
    %action = %this.createAction( ActionAddSequence, "Add sequence" );
    %action.seqName = %seqName;
    %action.origFrom = %from;
    %action.from = %from;
    %action.start = %start;
    %action.end = %end;

    %this.doAction( %action );
}

function ActionAddSequence::doit( %this ) {
    // If adding this sequence from an existing sequence, make a backup copy of
    // the existing sequence first, so we can edit the start/end frames later
    // without having to worry if the original source sequence has changed.
    if ( VehicleEditor.shape.getSequenceIndex( %this.from ) >= 0 ) {
        %this.from = VehicleEditor.getUniqueName( "sequence", "__backup__" @ %this.origFrom @ "_" );
        VehicleEditor.shape.addSequence( %this.origFrom, %this.from );
    }

    // Add the sequence
    $collada::forceLoadDAE = Lab.forceLoadDAE;
    %success = VehicleEditor.shape.addSequence( %this.from, %this.seqName, %this.start, %this.end );
    $collada::forceLoadDAE = false;

    if ( %success ) {
        VehicleEdPropWindow.update_onSequenceAdded( %this.seqName, -1 );
        return true;
    }
    return false;
}

function ActionAddSequence::undo( %this ) {
    Parent::undo( %this );

    // Remove the backup sequence if one was created
    if ( %this.origFrom !$= %this.from ) {
        VehicleEditor.shape.removeSequence( %this.from );
        %this.from = %this.origFrom;
    }

    // Remove the actual sequence
    if ( VehicleEditor.shape.removeSequence( %this.seqName ) )
        VehicleEdPropWindow.update_onSequenceRemoved( %this.seqName );
}

//------------------------------------------------------------------------------
// Remove sequence

function VehicleEditor::doRemoveSequence( %this, %seqName ) {
    %action = %this.createAction( ActionRemoveSequence, "Remove sequence" );
    %action.seqName = %seqName;

    %this.doAction( %action );
}

function ActionRemoveSequence::doit( %this ) {
    if ( VehicleEditor.shape.removeSequence( %this.seqName ) ) {
        VehicleEdPropWindow.update_onSequenceRemoved( %this.seqName );
        return true;
    }
    return false;
}

function ActionRemoveSequence::undo( %this ) {
    Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Rename sequence
function VehicleEditor::doRenameSequence( %this, %oldName, %newName ) {
    %action = %this.createAction( ActionRenameSequence, "Rename sequence" );
    %action.oldName = %oldName;
    %action.newName = %newName;

    %this.doAction( %action );
}

function ActionRenameSequence::doit( %this ) {
    if ( VehicleEditor.shape.renameSequence( %this.oldName, %this.newName ) ) {
        VehicleEdPropWindow.update_onSequenceRenamed( %this.oldName, %this.newName );
        return true;
    }
    return false;
}

function ActionRenameSequence::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.renameSequence( %this.newName, %this.oldName ) )
        VehicleEdPropWindow.update_onSequenceRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Edit sequence source data ( parent, start or end )
function VehicleEditor::doEditSeqSource( %this, %seqName, %from, %start, %end ) {
    %action = %this.createAction( ActionEditSeqSource, "Edit sequence source data" );
    %action.seqName = %seqName;
    %action.origFrom = %from;
    %action.from = %from;
    %action.start = %start;
    %action.end = %end;

    // To support undo, the sequence will be renamed instead of removed (undo just
    // removes the added sequence and renames the original back). Generate a unique
    // name for the backed up sequence
    %action.seqBackup = VehicleEditor.getUniqueName( "sequence", "__backup__" @ %action.seqName @  "_" );

    // If editing an internal sequence, the source is the renamed backup
    if ( %action.from $= %action.seqName )
        %action.from = %action.seqBackup;

    %this.doAction( %action );
}

function ActionEditSeqSource::doit( %this ) {
    // If changing the source to an existing sequence, make a backup copy of
    // the existing sequence first, so we can edit the start/end frames later
    // without having to worry if the original source sequence has changed.
    if ( !startswith( %this.from, "__backup__" ) &&
            VehicleEditor.shape.getSequenceIndex( %this.from ) >= 0 ) {
        %this.from = VehicleEditor.getUniqueName( "sequence", "__backup__" @ %this.origFrom @ "_" );
        VehicleEditor.shape.addSequence( %this.origFrom, %this.from );
    }

    // Get settings we want to retain
    %priority = VehicleEditor.shape.getSequencePriority( %this.seqName );
    %cyclic = VehicleEditor.shape.getSequenceCyclic( %this.seqName );
    %blend = VehicleEditor.shape.getSequenceBlend( %this.seqName );

    // Rename this sequence (instead of removing it) so we can undo this action
    VehicleEditor.shape.renameSequence( %this.seqName, %this.seqBackup );

    // Add the new sequence
    if ( VehicleEditor.shape.addSequence( %this.from, %this.seqName, %this.start, %this.end ) ) {
        // Restore original settings
        if ( VehicleEditor.shape.getSequencePriority ( %this.seqName ) != %priority )
            VehicleEditor.shape.setSequencePriority( %this.seqName, %priority );
        if ( VehicleEditor.shape.getSequenceCyclic( %this.seqName ) != %cyclic )
            VehicleEditor.shape.setSequenceCyclic( %this.seqName, %cyclic );

        %newBlend = VehicleEditor.shape.getSequenceBlend( %this.seqName );
        if ( %newBlend !$= %blend ) {
            // Undo current blend, then apply new one
            VehicleEditor.shape.setSequenceBlend( %this.seqName, 0, getField( %newBlend, 1 ), getField( %newBlend, 2 ) );
            if ( getField( %blend, 0 ) == 1 )
                VehicleEditor.shape.setSequenceBlend( %this.seqName, getField( %blend, 0 ), getField( %blend, 1 ), getField( %blend, 2 ) );
        }

        if ( VehicleEdSequenceList.getSelectedName() $= %this.seqName ) {
            VehicleEdSequenceList.editColumn( %this.seqName, 3, %this.end - %this.start + 1 );
            VehicleEdPropWindow.syncPlaybackDetails();
        }

        return true;
    }
    return false;
}

function ActionEditSeqSource::undo( %this ) {
    Parent::undo( %this );

    // Remove the source sequence backup if one was created
    if ( ( %this.from !$= %this.origFrom ) && ( %this.from !$= %this.seqBackup ) ) {
        VehicleEditor.shape.removeSequence( %this.from );
        %this.from = %this.origFrom;
    }

    // Remove the added sequence, and rename the backup back
    if ( VehicleEditor.shape.removeSequence( %this.seqName ) &&
            VehicleEditor.shape.renameSequence( %this.seqBackup, %this.seqName ) ) {
        if ( VehicleEdSequenceList.getSelectedName() $= %this.seqName ) {
            VehicleEdSequenceList.editColumn( %this.seqName, 3, %this.end - %this.start + 1 );
            VehicleEdPropWindow.syncPlaybackDetails();
        }
    }
}

//------------------------------------------------------------------------------
// Edit cyclic flag
function VehicleEditor::doEditCyclic( %this, %seqName, %cyclic ) {
    %action = %this.createAction( ActionEditCyclic, "Toggle cyclic flag" );
    %action.seqName = %seqName;
    %action.cyclic = %cyclic;

    %this.doAction( %action );
}

function ActionEditCyclic::doit( %this ) {
    if ( VehicleEditor.shape.setSequenceCyclic( %this.seqName, %this.cyclic ) ) {
        VehicleEdPropWindow.update_onSequenceCyclicChanged( %this.seqName, %this.cyclic );
        return true;
    }
    return false;
}

function ActionEditCyclic::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.setSequenceCyclic( %this.seqName, !%this.cyclic ) )
        VehicleEdPropWindow.update_onSequenceCyclicChanged( %this.seqName, !%this.cyclic );
}

//------------------------------------------------------------------------------
// Edit blend properties
function VehicleEditor::doEditBlend( %this, %seqName, %blend, %blendSeq, %blendFrame ) {
    %action = %this.createAction( ActionEditBlend, "Edit blend properties" );
    %action.seqName = %seqName;
    %action.blend = %blend;
    %action.blendSeq = %blendSeq;
    %action.blendFrame = %blendFrame;

    // Store the current blend settings
    %oldBlend = VehicleEditor.shape.getSequenceBlend( %seqName );
    %action.oldBlend = getField( %oldBlend, 0 );
    %action.oldBlendSeq = getField( %oldBlend, 1 );
    %action.oldBlendFrame = getField( %oldBlend, 2 );

    // Use new values if the old ones do not exist ( for blend sequences embedded
    // in the DTS/DSQ file )
    if ( %action.oldBlendSeq $= "" )
        %action.oldBlendSeq = %action.blendSeq;
    if ( %action.oldBlendFrame $= "" )
        %action.oldBlendFrame = %action.blendFrame;

    %this.doAction( %action );
}

function ActionEditBlend::doit( %this ) {
    // If we are changing the blend reference ( rather than just toggling the flag )
    // we need to undo the current blend first.
    if ( %this.blend && %this.oldBlend ) {
        if ( !VehicleEditor.shape.setSequenceBlend( %this.seqName, false, %this.oldBlendSeq, %this.oldBlendFrame ) )
            return false;
    }

    if ( VehicleEditor.shape.setSequenceBlend( %this.seqName, %this.blend, %this.blendSeq, %this.blendFrame ) ) {
        VehicleEdPropWindow.update_onSequenceBlendChanged( %this.seqName, %this.blend,
                %this.oldBlendSeq, %this.oldBlendFrame, %this.blendSeq, %this.blendFrame );
        return true;
    }
    return false;
}

function ActionEditBlend::undo( %this ) {
    Parent::undo( %this );

    // If we are changing the blend reference ( rather than just toggling the flag )
    // we need to undo the current blend first.
    if ( %this.blend && %this.oldBlend ) {
        if ( !VehicleEditor.shape.setSequenceBlend( %this.seqName, false, %this.blendSeq, %this.blendFrame ) )
            return;
    }

    if ( VehicleEditor.shape.setSequenceBlend( %this.seqName, %this.oldBlend, %this.oldBlendSeq, %this.oldBlendFrame ) ) {
        VehicleEdPropWindow.update_onSequenceBlendChanged( %this.seqName, !%this.blend,
                %this.blendSeq, %this.blendFrame, %this.oldBlendSeq, %this.oldBlendFrame );
    }
}

//------------------------------------------------------------------------------
// Edit sequence priority
function VehicleEditor::doEditSequencePriority( %this, %seqName, %newPriority ) {
    %action = %this.createAction( ActionEditSequencePriority, "Edit sequence priority" );
    %action.seqName = %seqName;
    %action.newPriority = %newPriority;
    %action.oldPriority = %this.shape.getSequencePriority( %seqName );

    %this.doAction( %action );
}

function ActionEditSequencePriority::doit( %this ) {
    if ( VehicleEditor.shape.setSequencePriority( %this.seqName, %this.newPriority ) ) {
        VehicleEdPropWindow.update_onSequencePriorityChanged( %this.seqName );
        return true;
    }
    return false;
}

function ActionEditSequencePriority::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.setSequencePriority( %this.seqName, %this.oldPriority ) )
        VehicleEdPropWindow.update_onSequencePriorityChanged( %this.seqName );
}

//------------------------------------------------------------------------------
// Edit sequence ground speed
function VehicleEditor::doEditSequenceGroundSpeed( %this, %seqName, %newSpeed ) {
    %action = %this.createAction( ActionEditSequenceGroundSpeed, "Edit sequence ground speed" );
    %action.seqName = %seqName;
    %action.newSpeed = %newSpeed;
    %action.oldSpeed = %this.shape.getSequenceGroundSpeed( %seqName );

    %this.doAction( %action );
}

function ActionEditSequenceGroundSpeed::doit( %this ) {
    if ( VehicleEditor.shape.setSequenceGroundSpeed( %this.seqName, %this.newSpeed ) ) {
        VehicleEdPropWindow.update_onSequenceGroundSpeedChanged( %this.seqName );
        return true;
    }
    return false;
}

function ActionEditSequenceGroundSpeed::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.setSequenceGroundSpeed( %this.seqName, %this.oldSpeed ) )
        VehicleEdPropWindow.update_onSequenceGroundSpeedChanged( %this.seqName );
}

//------------------------------------------------------------------------------
// Add trigger
function VehicleEditor::doAddTrigger( %this, %seqName, %frame, %state ) {
    %action = %this.createAction( ActionAddTrigger, "Add trigger" );
    %action.seqName = %seqName;
    %action.frame = %frame;
    %action.state = %state;

    %this.doAction( %action );
}

function ActionAddTrigger::doit( %this ) {
    if ( VehicleEditor.shape.addTrigger( %this.seqName, %this.frame, %this.state ) ) {
        VehicleEdPropWindow.update_onTriggerAdded( %this.seqName, %this.frame, %this.state );
        return true;
    }
    return false;
}

function ActionAddTrigger::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.removeTrigger( %this.seqName, %this.frame, %this.state ) )
        VehicleEdPropWindow.update_onTriggerRemoved( %this.seqName, %this.frame, %this.state );
}

//------------------------------------------------------------------------------
// Remove trigger
function VehicleEditor::doRemoveTrigger( %this, %seqName, %frame, %state ) {
    %action = %this.createAction( ActionRemoveTrigger, "Remove trigger" );
    %action.seqName = %seqName;
    %action.frame = %frame;
    %action.state = %state;

    %this.doAction( %action );
}

function ActionRemoveTrigger::doit( %this ) {
    if ( VehicleEditor.shape.removeTrigger( %this.seqName, %this.frame, %this.state ) ) {
        VehicleEdPropWindow.update_onTriggerRemoved( %this.seqName, %this.frame, %this.state );
        return true;
    }
    return false;
}

function ActionRemoveTrigger::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.addTrigger( %this.seqName, %this.frame, %this.state ) )
        VehicleEdPropWindow.update_onTriggerAdded( %this.seqName, %this.frame, %this.state );
}

//------------------------------------------------------------------------------
// Edit trigger
function VehicleEditor::doEditTrigger( %this, %seqName, %oldFrame, %oldState, %frame, %state ) {
    %action = %this.createAction( ActionEditTrigger, "Edit trigger" );
    %action.seqName = %seqName;
    %action.oldFrame = %oldFrame;
    %action.oldState = %oldState;
    %action.frame = %frame;
    %action.state = %state;

    %this.doAction( %action );
}

function ActionEditTrigger::doit( %this ) {
    if ( VehicleEditor.shape.addTrigger( %this.seqName, %this.frame, %this.state ) &&
            VehicleEditor.shape.removeTrigger( %this.seqName, %this.oldFrame, %this.oldState ) ) {
        VehicleEdTriggerList.updateItem( %this.oldFrame, %this.oldState, %this.frame, %this.state );
        return true;
    }
    return false;
}

function ActionEditTrigger::undo( %this ) {
    Parent::undo( %this );

    if ( VehicleEditor.shape.addTrigger( %this.seqName, %this.oldFrame, %this.oldState ) &&
            VehicleEditor.shape.removeTrigger( %this.seqName, %this.frame, %this.state ) )
        VehicleEdTriggerList.updateItem( %this.frame, %this.state, %this.oldFrame, %this.oldState );
}

//------------------------------------------------------------------------------
// Rename detail
function VehicleEditor::doRenameDetail( %this, %oldName, %newName ) {
    %action = %this.createAction( ActionRenameDetail, "Rename detail" );
    %action.oldName = %oldName;
    %action.newName = %newName;

    %this.doAction( %action );
}

function ActionRenameDetail::doit( %this ) {
    if ( VehicleEditor.shape.renameDetailLevel( %this.oldName, %this.newName ) ) {
        VehicleEdPropWindow.update_onDetailRenamed( %this.oldName, %this.newName );
        return true;
    }
    return false;
}

function ActionRenameDetail::undo( %this ) {
    Parent::undo( %this );
    if ( VehicleEditor.shape.renameDetailLevel( %this.newName, %this.oldName ) )
        VehicleEdPropWindow.update_onDetailRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Edit detail size
function VehicleEditor::doEditDetailSize( %this, %oldSize, %newSize ) {
    %action = %this.createAction( ActionEditDetailSize, "Edit detail size" );
    %action.oldSize = %oldSize;
    %action.newSize = %newSize;

    %this.doAction( %action );
}

function ActionEditDetailSize::doit( %this ) {
    %dl = VehicleEditor.shape.setDetailLevelSize( %this.oldSize, %this.newSize );
    if ( %dl != -1 ) {
        VehicleEdPropWindow.update_onDetailSizeChanged( %this.oldSize, %this.newSize );
        return true;
    }
    return false;
}

function ActionEditDetailSize::undo( %this ) {
    Parent::undo( %this );
    %dl = VehicleEditor.shape.setDetailLevelSize( %this.newSize, %this.oldSize );
    if ( %dl != -1 )
        VehicleEdPropWindow.update_onDetailSizeChanged( %this.newSize, %this.oldSize );
}

//------------------------------------------------------------------------------
// Rename object
function VehicleEditor::doRenameObject( %this, %oldName, %newName ) {
    %action = %this.createAction( ActionRenameObject, "Rename object" );
    %action.oldName = %oldName;
    %action.newName = %newName;

    %this.doAction( %action );
}

function ActionRenameObject::doit( %this ) {
    if ( VehicleEditor.shape.renameObject( %this.oldName, %this.newName ) ) {
        VehicleEdPropWindow.update_onObjectRenamed( %this.oldName, %this.newName );
        return true;
    }
    return false;
}

function ActionRenameObject::undo( %this ) {
    Parent::undo( %this );
    if ( VehicleEditor.shape.renameObject( %this.newName, %this.oldName ) )
        VehicleEdPropWindow.update_onObjectRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Edit mesh size
function VehicleEditor::doEditMeshSize( %this, %meshName, %size ) {
    %action = %this.createAction( ActionEditMeshSize, "Edit mesh size" );
    %action.meshName = stripTrailingNumber( %meshName );
    %action.oldSize = getTrailingNumber( %meshName );
    %action.newSize = %size;

    %this.doAction( %action );
}

function ActionEditMeshSize::doit( %this ) {
    if ( VehicleEditor.shape.setMeshSize( %this.meshName SPC %this.oldSize, %this.newSize ) ) {
        VehicleEdPropWindow.update_onMeshSizeChanged( %this.meshName, %this.oldSize, %this.newSize );
        return true;
    }
    return false;
}

function ActionEditMeshSize::undo( %this ) {
    Parent::undo( %this );
    if ( VehicleEditor.shape.setMeshSize( %this.meshName SPC %this.newSize, %this.oldSize ) )
        VehicleEdPropWindow.update_onMeshSizeChanged( %this.meshName, %this.oldSize, %this.oldSize );
}

//------------------------------------------------------------------------------
// Edit billboard type
function VehicleEditor::doEditMeshBillboard( %this, %meshName, %type ) {
    %action = %this.createAction( ActionEditMeshBillboard, "Edit mesh billboard" );
    %action.meshName = %meshName;
    %action.oldType = %this.shape.getMeshType( %meshName );
    %action.newType = %type;

    %this.doAction( %action );
}

function ActionEditMeshBillboard::doit( %this ) {
    if ( VehicleEditor.shape.setMeshType( %this.meshName, %this.newType ) ) {
        switch$ ( VehicleEditor.shape.getMeshType( %this.meshName ) ) {
        case "normal":
            VehicleEdDetails-->bbType.setSelected( 0, false );
        case "billboard":
            VehicleEdDetails-->bbType.setSelected( 1, false );
        case "billboardzaxis":
            VehicleEdDetails-->bbType.setSelected( 2, false );
        }
        return true;
    }
    return false;
}

function ActionEditMeshBillboard::undo( %this ) {
    Parent::undo( %this );
    if ( VehicleEditor.shape.setMeshType( %this.meshName, %this.oldType ) ) {
        %id = VehicleEdDetailTree.getSelectedItem();
        if ( ( %id > 1 ) && ( VehicleEdDetailTree.getItemText( %id ) $= %this.meshName ) ) {
            switch$ ( VehicleEditor.shape.getMeshType( %this.meshName ) ) {
            case "normal":
                VehicleEdDetails-->bbType.setSelected( 0, false );
            case "billboard":
                VehicleEdDetails-->bbType.setSelected( 1, false );
            case "billboardzaxis":
                VehicleEdDetails-->bbType.setSelected( 2, false );
            }
        }
    }
}

//------------------------------------------------------------------------------
// Edit object node
function VehicleEditor::doSetObjectNode( %this, %objName, %node ) {
    %action = %this.createAction( ActionSetObjectNode, "Set object node" );
    %action.objName = %objName;
    %action.oldNode = %this.shape.getObjectNode( %objName );
    %action.newNode = %node;

    %this.doAction( %action );
}

function ActionSetObjectNode::doit( %this ) {
    if ( VehicleEditor.shape.setObjectNode( %this.objName, %this.newNode ) ) {
        VehicleEdPropWindow.update_onObjectNodeChanged( %this.objName );
        return true;
    }
    return false;
}

function ActionSetObjectNode::undo( %this ) {
    Parent::undo( %this );
    if ( VehicleEditor.shape.setObjectNode( %this.objName, %this.oldNode ) )
        VehicleEdPropWindow.update_onObjectNodeChanged( %this.objName );
}

//------------------------------------------------------------------------------
// Remove mesh
function VehicleEditor::doRemoveMesh( %this, %meshName ) {
    %action = %this.createAction( ActionRemoveMesh, "Remove mesh" );
    %action.meshName = %meshName;

    %this.doAction( %action );
}

function ActionRemoveMesh::doit( %this ) {
    if ( VehicleEditor.shape.removeMesh( %this.meshName ) ) {
        VehicleEdPropWindow.update_onMeshRemoved( %this.meshName );
        return true;
    }
    return false;
}

function ActionRemoveMesh::undo( %this ) {
    Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Add meshes from file
function VehicleEditor::doAddMeshFromFile( %this, %filename, %size ) {
    %action = %this.createAction( ActionAddMeshFromFile, "Add mesh from file" );
    %action.filename = %filename;
    %action.size = %size;

    %this.doAction( %action );
}

function ActionAddMeshFromFile::doit( %this ) {
    %this.meshList = VehicleEditor.addLODFromFile( VehicleEditor.shape, %this.filename, %this.size, 1 );
    if ( %this.meshList !$= "" ) {
        %count = getFieldCount( %this.meshList );
        for ( %i = 0; %i < %count; %i++ )
            VehicleEdPropWindow.update_onMeshAdded( getField( %this.meshList, %i ) );

        VehicleEdMaterials.updateMaterialList();

        return true;
    }
    return false;
}

function ActionAddMeshFromFile::undo( %this ) {
    // Remove all the meshes we added
    %count = getFieldCount( %this.meshList );
    for ( %i = 0; %i < %count; %i ++ ) {
        %name = getField( %this.meshList, %i );
        VehicleEditor.shape.removeMesh( %name );
        VehicleEdPropWindow.update_onMeshRemoved( %name );
    }
    VehicleEdMaterials.updateMaterialList();
}

//------------------------------------------------------------------------------
// Add/edit collision geometry
function VehicleEditor::doEditCollision( %this, %type, %target, %depth, %merge, %concavity,
                                       %maxVerts, %boxMax, %sphereMax, %capsuleMax ) {
    %colData = VehicleEdColWindow.lastColSettings;

    %action = %this.createAction( ActionEditCollision, "Edit shape collision" );

    %action.oldType = getField( %colData, 0 );
    %action.oldTarget = getField( %colData, 1 );
    %action.oldDepth = getField( %colData, 2 );
    %action.oldMerge = getField( %colData, 3 );
    %action.oldConcavity = getField( %colData, 4 );
    %action.oldMaxVerts = getField( %colData, 5 );
    %action.oldBoxMax = getField( %colData, 6 );
    %action.oldSphereMax = getField( %colData, 7 );
    %action.oldCapsuleMax = getField( %colData, 8 );

    %action.newType = %type;
    %action.newTarget = %target;
    %action.newDepth = %depth;
    %action.newMerge = %merge;
    %action.newConcavity = %concavity;
    %action.newMaxVerts = %maxVerts;
    %action.newBoxMax = %boxMax;
    %action.newSphereMax = %sphereMax;
    %action.newCapsuleMax = %capsuleMax;

    %this.doAction( %action );
}

function ActionEditCollision::updateCollision( %this, %type, %target, %depth, %merge, %concavity,
        %maxVerts, %boxMax, %sphereMax, %capsuleMax ) {
    %colDetailSize = -1;
    %colNode = "Col" @ %colDetailSize;

    // TreeView items are case sensitive, but TSShape names are not, so fixup case
    // if needed
    %index = VehicleEditor.shape.getNodeIndex( %colNode );
    if ( %index != -1 )
        %colNode = VehicleEditor.shape.getNodeName( %index );

    // First remove the old detail and collision nodes
    %meshList = VehicleEditor.getDetailMeshList( %colDetailSize );
    %meshCount = getFieldCount( %meshList );
    if ( %meshCount > 0 ) {
        VehicleEditor.shape.removeDetailLevel( %colDetailSize );
        for ( %i = 0; %i < %meshCount; %i++ )
            VehicleEdPropWindow.update_onMeshRemoved( getField( %meshList, %i ) );
    }

    %nodeList = VehicleEditor.getNodeNames( %colNode, "" );
    %nodeCount = getFieldCount( %nodeList );
    if ( %nodeCount > 0 ) {
        for ( %i = 0; %i < %nodeCount; %i++ )
            VehicleEditor.shape.removeNode( getField( %nodeList, %i ) );
        VehicleEdPropWindow.update_onNodeRemoved( %nodeList, %nodeCount );
    }

    // Add the new node and geometry
    if ( %type $= "" )
        return;

    if ( !VehicleEditor.shape.addCollisionDetail( %colDetailSize, %type, %target,
            %depth, %merge, %concavity, %maxVerts,
            %boxMax, %sphereMax, %capsuleMax ) )
        return false;

    // Update UI
    %meshList = VehicleEditor.getDetailMeshList( %colDetailSize );
    VehicleEdPropWindow.update_onNodeAdded( %colNode, VehicleEditor.shape.getNodeCount() );    // will also add child nodes
    %count = getFieldCount( %meshList );
    for ( %i = 0; %i < %count; %i++ )
        VehicleEdPropWindow.update_onMeshAdded( getField( %meshList, %i ) );

    VehicleEdColWindow.lastColSettings = %type TAB %target TAB %depth TAB %merge TAB
                                       %concavity TAB %maxVerts TAB %boxMax TAB %sphereMax TAB %capsuleMax;
    VehicleEdColWindow.update_onCollisionChanged();

    return true;
}

function ActionEditCollision::doit( %this ) {
    VehicleEdWaitGui.show( "Generating collision geometry..." );
    %success = %this.updateCollision( %this.newType, %this.newTarget, %this.newDepth, %this.newMerge,
                                      %this.newConcavity, %this.newMaxVerts, %this.newBoxMax,
                                      %this.newSphereMax, %this.newCapsuleMax );
    VehicleEdWaitGui.hide();

    return %success;
}

function ActionEditCollision::undo( %this ) {
    Parent::undo( %this );

    VehicleEdWaitGui.show( "Generating collision geometry..." );
    %this.updateCollision( %this.oldType, %this.oldTarget, %this.oldDepth, %this.oldMerge,
                           %this.oldConcavity, %this.oldMaxVerts, %this.oldBoxMax,
                           %this.oldSphereMax, %this.oldCapsuleMax );
    VehicleEdWaitGui.hide();
}

//------------------------------------------------------------------------------
// Remove Detail

function VehicleEditor::doRemoveDetail( %this, %size ) {
    %action = %this.createAction( ActionRemoveDetail, "Remove detail level" );
    %action.size = %size;

    %this.doAction( %action );
}

function ActionRemoveDetail::doit( %this ) {
    %meshList = VehicleEditor.getDetailMeshList( %this.size );
    if ( VehicleEditor.shape.removeDetailLevel( %this.size ) ) {
        %meshCount = getFieldCount( %meshList );
        for ( %i = 0; %i < %meshCount; %i++ )
            VehicleEdPropWindow.update_onMeshRemoved( getField( %meshList, %i ) );
        return true;
    }
    return false;
}

function ActionRemoveDetail::undo( %this ) {
    Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Update bounds
function VehicleEditor::doSetBounds( %this ) {
    %action = %this.createAction( ActionSetBounds, "Set bounds" );
    %action.oldBounds = VehicleEditor.shape.getBounds();
    %action.newBounds = VehicleEdShapeView.computeShapeBounds();

    %this.doAction( %action );
}

function ActionSetBounds::doit( %this ) {
    return VehicleEditor.shape.setBounds( %this.newBounds );
}

function ActionSetBounds::undo( %this ) {
    Parent::undo( %this );

    VehicleEditor.shape.setBounds( %this.oldBounds );
}

//------------------------------------------------------------------------------
// Add/edit imposter
function VehicleEditor::doEditImposter( %this, %dl, %detailSize, %bbEquatorSteps, %bbPolarSteps,
                                      %bbDetailLevel, %bbDimension, %bbIncludePoles, %bbPolarAngle ) {
    %action = %this.createAction( ActionEditImposter, "Edit imposter" );
    %action.oldDL = %dl;
    if ( %action.oldDL != -1 ) {
        %action.oldSize = VehicleEditor.shape.getDetailLevelSize( %dl );
        %action.oldImposter = VehicleEditor.shape.getImposterSettings( %dl );
    }
    %action.newSize = %detailSize;
    %action.newImposter = "1" TAB %bbEquatorSteps TAB %bbPolarSteps TAB %bbDetailLevel TAB
                          %bbDimension TAB %bbIncludePoles TAB %bbPolarAngle;

    %this.doAction( %action );
}

function ActionEditImposter::doit( %this ) {
    // Unpack new imposter settings
    for ( %i = 0; %i < 7; %i++ )
        %val[%i] = getField( %this.newImposter, %i );

    VehicleEdWaitGui.show( "Generating imposter bitmaps..." );

    // Need to de-highlight the current material, or the imposter will have the
    // highlight effect baked in!
    VehicleEdMaterials.updateSelectedMaterial( false );

    %dl = VehicleEditor.shape.addImposter( %this.newSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
    VehicleEdWaitGui.hide();

    // Restore highlight effect
    VehicleEdMaterials.updateSelectedMaterial( VehicleEdMaterials-->highlightMaterial.getValue() );

    if ( %dl != -1 ) {
        VehicleEdShapeView.refreshShape();
        VehicleEdShapeView.currentDL = %dl;
        VehicleEdAdvancedWindow-->detailSize.setText( %this.newSize );
        VehicleEdDetails-->meshSize.setText( %this.newSize );
        VehicleEdDetails.update_onDetailsChanged();

        return true;
    }
    return false;
}

function ActionEditImposter::undo( %this ) {
    Parent::undo( %this );

    // If this was a new imposter, just remove it. Otherwise restore the old settings
    if ( %this.oldDL < 0 ) {
        if ( VehicleEditor.shape.removeImposter() ) {
            VehicleEdShapeView.refreshShape();
            VehicleEdShapeView.currentDL = 0;
            VehicleEdDetails.update_onDetailsChanged();
        }
    } else {
        // Unpack old imposter settings
        for ( %i = 0; %i < 7; %i++ )
            %val[%i] = getField( %this.oldImposter, %i );

        VehicleEdWaitGui.show( "Generating imposter bitmaps..." );

        // Need to de-highlight the current material, or the imposter will have the
        // highlight effect baked in!
        VehicleEdMaterials.updateSelectedMaterial( false );

        %dl = VehicleEditor.shape.addImposter( %this.oldSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
        VehicleEdWaitGui.hide();

        // Restore highlight effect
        VehicleEdMaterials.updateSelectedMaterial( VehicleEdMaterials-->highlightMaterial.getValue() );

        if ( %dl != -1 ) {
            VehicleEdShapeView.refreshShape();
            VehicleEdShapeView.currentDL = %dl;
            VehicleEdAdvancedWindow-->detailSize.setText( %this.oldSize );
            VehicleEdDetails-->meshSize.setText( %this.oldSize );
        }
    }
}

//------------------------------------------------------------------------------
// Remove imposter
function VehicleEditor::doRemoveImposter( %this ) {
    %action = %this.createAction( ActionRemoveImposter, "Remove imposter" );
    %dl = VehicleEditor.shape.getImposterDetailLevel();
    if ( %dl != -1 ) {
        %action.oldSize = VehicleEditor.shape.getDetailLevelSize( %dl );
        %action.oldImposter = VehicleEditor.shape.getImposterSettings( %dl );
        %this.doAction( %action );
    }
}

function ActionRemoveImposter::doit( %this ) {
    if ( VehicleEditor.shape.removeImposter() ) {
        VehicleEdShapeView.refreshShape();
        VehicleEdShapeView.currentDL = 0;
        VehicleEdDetails.update_onDetailsChanged();

        return true;
    }
    return false;
}

function ActionRemoveImposter::undo( %this ) {
    Parent::undo( %this );

    // Unpack the old imposter settings
    for ( %i = 0; %i < 7; %i++ )
        %val[%i] = getField( %this.oldImposter, %i );

    VehicleEdWaitGui.show( "Generating imposter bitmaps..." );
    %dl = VehicleEditor.shape.addImposter( %this.oldSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
    VehicleEdWaitGui.hide();

    if ( %dl != -1 ) {
        VehicleEdShapeView.refreshShape();
        VehicleEdShapeView.currentDL = %dl;
        VehicleEdAdvancedWindow-->detailSize.setText( %this.oldSize );
        VehicleEdDetails-->meshSize.setText( %this.oldSize );
        VehicleEdDetails.update_onDetailsChanged();
    }
}
