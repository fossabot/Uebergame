singleton CubemapData( spacebox_01_Cubemap )
{
   cubeFace[0] = "./spacebox_01";
   cubeFace[1] = "./spacebox_01";
   cubeFace[2] = "./spacebox_01";
   cubeFace[3] = "./spacebox_01";
   cubeFace[4] = "./spacebox_01";
   cubeFace[5] = "./spacebox_01";
};

singleton Material( spacebox_01_Mat )
{
   cubemap = spacebox_01_Cubemap;
   materialTag0 = "Skies";
};
