LowPoly Terrain - Track (2020/3/26 - by Fatty War, id3644@gmail.com)

[Update V2.00 (2022/3/17)]
-Added textured track. (track only / excluding prop and downhill courses). (See section 9)
    Textured tracks support uv coordinates in meshes, so you can use textures.
-Three-way intersection, straight hill track has been added.(See section 10)
-30, 60 degree corner (band) track added, not compatible with the module.(Added by user request)(See section 10)

[Update V1.06 2nd (2020/8/27)]
-Improved compatibility of track parts.("Bg_54x54W10A00BandSide" track is connected to "Bg_54x54W10A00DCL" & "Bg_54x54W10A00DCR").
-A rock slope has been added. ("Bg_54SlopeA00CN01" - model & prefab / corner negative Resist)
-The "FattyGroundTool" script has been moved to the "Editor" folder (removed the cause of the build error).
-Some prefabs position fix

[Update V1.06(2020/7/26)]
-The existing 50M module size has been changed to 54M. (Compatible with 18M module package)
-If you need a 50M module, please contact us by email.

[Update V1.05(2020/6/14)]
-A tropical beach theme was added. (Includes inter-terrain transition blocks/See section 8)
-Fixed "Col_50x50GroundOver" collider

[Update V1.04(2020/6/4)]
-Infinite runner track & prefab have been added.(See section 7)
     -course example scene has been added.(See section 4)
     -demo script was added.(See section 3-Script for Infinite Runner)
-Slope module added
     -Slope templates decorated with wood and stones has been added.(See section 1-3, 2-5)
-Move Tool
     -A lift function function has been added (directional pad).
     -Create near the screen, the selected track or the final track (Add Track Prefab button).
     -Added thumbnail name filter function.
     -Fixed snap error.

[Update V1.03(2020/5/18)]
-Added Slope. (See section 6)
     -You can create a slope on the track.
     -You can assemble terrain with different heights.
     -Several environmental props have been added.

[Update V1.02(2020/5/1)]
-Added Stunt Prototype tracks.
     -40 Stunt Prototype tracks / none scale. (See section 5)
     -Stunt track example scene.
     -Preview stunt scripts.(See section 3)

-Fixed some prefabs position.
     -Bg_50x50M00_1x1Grass00, Bg_50x50M00_1x1Hill, Bg_50x50M00_2x1Hill, Bg_50x50T00C

[Update V1.01(2020/4/19)]
-Added 2 terrainless tracks.(Dirt Road(13), Asphalt(13), Mix Cross(2))
-Added auxiliary tools for assembly.(This is a test version - Use the tool after testing it in the practice scene, never test it in the main task.)


===============This document explains how to use the pack.====================

Introduction

    Track kit for low poly art.
	Easily assemble your favorite tracks with drag and drop.
	Modules are blocks that combine roads and terrain. (54 by 54)
	The track is 10 unit/unity wide (various widths will be updated in the future).
	
	Note).
	-All meshes used only one material & texture. Each mesh has its own UV for each color.
	-Track width and type can be added by feedback. Please leave feedback in the comments.

1. Folder description

     1-1. Meshs(Assets\LowPolyTerrain-Track\Meshs)
          Contains the fbx just exported from 3dmax.

     1-2. Models(Assets\LowPolyTerrain-Track\Models)
          Contains a basic prefab with only material linked.(with Collider, Collider is sometimes absent. Ex - grass, decal.)

               \Bg. (Assets\LowPolyTerrain-Track\Models\Bg) - Contains environmental props used as a background.(with Material & Collider)
               \DownhillCourse. (Assets\LowPolyTerrain-Track\Models\DownhillCourse) - Contains a broken slope road. (with Material & Collider)
               \Slope. (Assets\LowPolyTerrain-Track\Models\Slope) - There are slope modules that connect different height terrain.(with Material & Collider)
               \StuntPrototype. (Assets\LowPolyTerrain-Track\Models\StuntPrototype) - Stunt foots, loops and obstacle shapes. (with Material & Collider / The stunt track's scale is independent of the track.)
               \Track. (Assets\LowPolyTerrain-Track\Models\Track) - Contains an empty track ground. (with Material & Collider)
               *\Ground. (Assets\LowPolyTerrain-Track\Models\Ground) - There are trackless terrain and transition blocks.

     1-3. Prefabs(Assets\LowPolyTerrain-Track\Prefabs)
          Contains a complex prefab assembled as a basic prefab in the Models folder.(Basic models may be combined, or scripts, special shaders may be combined.)

               \Downhill(M)odules. (Assets\LowPolyTerrain-Track\Prefabs\DownhillM) - Contains a prefab for the preview video "Downhill" displayed in the store product information. (If you do not want a broken road, please suspend use.)
               \StuntPrototype. (Assets\LowPolyTerrain-Track\Prefabs\StuntPrototype) - Contains a prefab for the preview video "stunt" displayed in the store product information.
               \Track(M)odules. (Assets\LowPolyTerrain-Track\Prefabs\TrackM) - There is a  decorated track module. Drag and drop to assemble tracks.
               \Slope(M)odules. (Assets\LowPolyTerrain-Track\Prefabs\SlopeM)- There is a decorated slope module. Assemble the slope by drag and drop.

2.File Naming
     2-1 Track. (Assets\LowPolyTerrain-Track\Models\Track)
	V1.05 Note) As of 1.04, the "Track" string has been removed from the track mesh name.

	ex) Bg_54x54W10TrackA00 + extension. (A + B)

	-A
	 1. 54x54 Modular, Road Wide 10, A Thema Track(Forest).
	 2. 54x54 Modular, Road Wide 10, B Thema Track(Road track without terrain.).
	 3. 54x54 Modular, Road Wide 10, C Thema Track(Asphalt track without terrain.).
	 *4. 54x54 Modular, Road Wide 10, D Thema Track(Tropical beach).

	-B
	 1. Empty = Straight.
	 2. C = Corner.
	 3. D = Diagonal.
	 4. E = End.
	 5. DL = Diagonal Left.
	 6. DLS = Diagonal Left Part Start.
	 7. DR = Diagonal Right.
	 8. DRS = Diagonal Right Part Start.
	 9. DL45 = Diagonal Turn Left.
	 10. DR45 = Diagonal Turn Right.
	 11. DCL = Diagonal Corner Left.
	 12. DCR = Diagonal Corner Right.
	 13. X = Cross
	 14. XOver = Cross(Overpass)
	 15. BandL = Band Left. (Straight Module Piaces)
	 16. BandLReturn = Band Left Return. (Straight Module Piaces)
	 17. BandR = Band Right. (Straight Module Piaces)
	 18. BandRReturn = Band Right Return. (Straight Module Piaces)
	 19. BandSide = One Side Track (Straight Module Piaces)
	 
	 
     2-2 DownhillCourse. (Assets\LowPolyTerrain-Track\Models\DownhillCourse)
             *Some roads are cut off for stunts.

             ex) Bg_DHCourseA + extension. (A + B)
             
             -A
             1. 54x54 Modular, Road Wide 10, A Thema Course.
             2. 54x54 Modular, Road Wide 10, A Thema Jump.
             
             -B
             1. I = Straight.
             2. JB = (J) letter shape, (B)roken.
             3. LB = (L) letter shape, (B)roken.
             4. ZB = (Z) letter shape, (B)roken.
             
     2-3 Slope, (Assets\LowPolyTerrain-Track\Models\Slope)
             You can assemble terrain with different heights.
             There are no roads on the slopes, only ramps.

             ex) Bg_DHSlopeA00 + extension (A + B)
             
             -A
             1. 54x54 Modular, Road Wide 10, A Thema Slope(Forest).
             2. 54x54 Modular, Road Wide 10, A Thema Bridge(Forest).
             *3. 54x54 Modular, Road Wide 10, D Thema Slope(Tropical beach).
             
             -B
             1. I = Side Slope.
	 2. C = (C)orner Slope.
	 3. CN = (C)orner (N)egative Resist.
	 4. D = (D)iagonal Side Slope.
	 5. R = (R)amp road Slope.
	 6. RE = (R)amp (E)nd road Slope.

     *2-4 Ground. (Assets\LowPolyTerrain-Track\Models\Ground)
             Build trackless terrain.
             
             ex) Bg_54x54GroundD00
             
             1. GroundD00 = Beach Thema Ground
             2. DtoA00 = Beach and forest transition blocks.
             3. Extra = Although not common, it is used for special conditions.
             
     2-5 Downhill(M)odule. (Assets\LowPolyTerrain-Track\Prefabs\DownhillM)
             You can assemble terrain with different heights.
             There are no roads on the slopes, only ramps.

             ex) Bg_DHCliff01 + extension (A + B)
             -A
              Downhill Cliff
              Downhill Slope

             -B
             1. Empty = Basic terrain without roads.
             2. I = Straight course downhill..
	 3. J = (J) course downhill.
	 4. L = (L) course downhill.
	 5. Z = (Z) course downhill.
	 6. Jump = Jump course downhill.
	 7. Three = Triple jump downhill

     2-6 Slope(M)odule. (Assets\LowPolyTerrain-Track\Prefabs\SlopeM)
             Create tracks for infinite runner-like games.
             Straight and curved roads are included.

             ex) Bg_DHS00 + extension (A + B)
             -A
              50x50 Modular Slpoe Set

             -B
             1. I = Side Slope.
	 2. C = (C)orner Slope.
	 3. CN = (C)orner (N)egative Resist.
	 4. D = (D)iagonal Side Slope.
	 5. R = (R)amp road Slope.
	 6. RE = (R)amp (E)nd road Slope.

     2-7 Runner(M)odule. (Assets\LowPolyTerrain-Track\Prefabs\RunnerM)
             Create tracks for infinite runner-like games.
             Straight and curved roads are included.

             ex) Run_50M + extension (A + B)
             -A
              Run_54M = Infinite runner 54M track.
              Run_162M = Infinite runner 162M track.
              Run_x~ = Infinite runner Obstacle.
              *Run_B54M = Infinite runner 54M track.

             -B
             1. Flat = Flat Module.
             2. FlattoHill = Flat to Hill Blend Module.
	 3. FlattoCliff = Flat to Cliff Blend Module.
	 4. Cliff = Cliff Module.
	 5. ClifftoFlat = Cliff to Flat Blend Module.
	 6. ClifftoHill = Cliff to Hill Blend Module.
	 7. Hill = Hill Module.
	 8. HilltoCliff = HilltoCliff Blend Module.
	 9. HilltoFlat = HilltoFlat Blend Module.
	 10. BandLeft = Left biased road set (case 150)
	 11. BandRight = Right biased road set (case 150)
	 
	 
3.Scripts. (Assets\ModularTrackKit\Scripts)
	
	*EightDirectionMovement.cs
	   -Move script for demo scene.

	*FollowTarget.cs
	   -Camera script for the demo scene.

	*FattyGroundTool.cs
		This is a test version - Use the tool after testing it in the practice scene.(never test it in the main task.)
		It is a tool to assist in assembling the track(Ground).
		It is helpful for mass assembly work.
		-It has simple move, rotate, copy and delete functions.
		-Snap function.
		-Track pallet registration function.

		How to use.
			1. Open a tool pop-up window.(Click Editor menu\Windows\PetieGroundTool)
			2. Enter the snap setting.
			3. In the Hierarchy window, select the prefab you want to assemble. (In the case of a game object, the status window will turn green.)
			4. Move and rotate the prefab with the directional pad to assemble it.
			5. Done
			
		Track palette
			You can register the prefab and easily take it out with the thumbnail image button.

			Prefab registration (palette)
				1. Automatic registration
				2. Manual registration

					Automatic registration
					1. Drag and drop the folder containing the prefab you want to register into the folder path slot.(Then the autofill button is activated.)
					2. You can check the contents of the folder in the information window.
					3. If you press the auto-fill button, the contents in the folder are automatically registered and a thumbnail is created and displayed as a button.
					4. Done - Now you can easily take out the prefab with the thumbnail button.

					Manual registration
					1. Enter the quantity you want to register in "Ground Size".(Slots are expanded by the number entered.)
					2. Drag and drop each desired prefab into the slot.
					3. In the activated slot, a thumbnail is created and the button is activated.
					4. Done - Use it to pull out a prefab with an activated button.

		-Please refer to the online documentation for details.(https://docs.google.com/document/d/1EUexHjNWlgp9VL8DGBfqIodOnmVYrDvNuaFFm2gtGMQ/edit?usp=sharing)

		

		*Script for stunt tracks.
			This is a script for previewing.

			-TouchHoldAccelerate.cs
				-It is a car touch controller.
				-The car rushes when you touch the screen.
				-When the car is in the air, touch the screen to rotate the car. (Run aerial acrobatics or level balance)
			
			-StuntTrap.cs
				-Stunttrack obstacle script.
				-Equipped with a prefab that prevents sprinting in the preview scene.

		*Script for Downhill
			This is a script for previewing.

			-SimpleSteering.cs
			*Bike steering controller.("A"-left, "D"-right, "Space Bar" Press-wheelie, "Space Bar" up-jump)

			-DownhillGem.cs
			*When a collider hits it, it brings in the particles and disappears.

		*Script for Infinite Runner
			This is a script for previewing.

			-SimpleRunner.cs
			*Bike controller.(Swipe the screen with the mouse = move left, right, jump, sliding.)


4.Example Scene(Assets\ModularTrackKit\Scenes)

	-TrackDemo
       -Includes 12 template tracks.
	   -You can preview the track by pressing the editor play button and using WASD.

	-TrackDisplay
	   -Prefabs used in the package are on display.

	-TrackDemo-CourseExample
		-Contains 15 course examples.

	-TrackDemo-Promotion
		-A large track for promotional images.
		-It is not recommended for use in game production.

	-TrackDemoStunt(V1.02 Added)
		-Stunt track preview scene.
		-You can preview 40 track parts and 34 prefabs (prefabs are examples of use).

	-SlopeDemo_Downhill(V1.03 Added)
		-Downhill preview scene : Press Play in the editor to enjoy the ride.
		-Slide through a dangerous, broken ramp. ("A"-left, "D"-right. / Do not go off the slope.)
		-Jump on a broken ramp. ("Space Bar" Press-wheelie, "Space Bar" up-jump / If you're lucky, you can get gems in the air.)
		-It will end when you reach all the slopes to the finish line...  but, it's not game, just preview. Enjoy!

	-TrackDemo-Runner50(V1.04 Added)
		-This is an infinite runner demo scene for 50 meter modules.
		-Swipe the screen to move the bike.

	-TrackDemo-Runner150(V1.04 Added)
		-This is an demo scene for 150 meter modules.
		-The winding path is not implemented in the script.

	-TrackDemo-RunnerTileBlend(V1.04 Added)
		-Contains examples of mixing between tracks.

	-TrackRunnnerDisplay(V1.04 Added)
		-Prefabs used in the package are on display.

	-TrackDisplayTropicalBeach(V1.05 Added)
		-Prefabs used in the package are on display.


5. Stunt Track (V1.02 Added)
	Contains track parts to create prototypes of vehicle stunt types.
	The part consists of 40 types.
	Stunt demo scenes are available for your understanding.
	In the demo scene, vehicle control and 7 types of obstacles are also implemented.

	Note) The track is non-scale. (For reference, the track thickness is 0.1 unit / unity.)

	5-1.Track Prefab List.
        0. dummy bike
        1. (B)ar000. (axis center)
        2. (B)ar012. (axis center)
        3. (O)Ring 000. (axis center)
        4. (C)ircle 001Quarter. (axis lower left)
        5. (C)ircle 002. (axis center)
        6. (C)ircle 002 half. (axis lower middle)
        7. (C)ircle 003 half. (axis lower middle)
        8. (C)ircle 004 Bracket. (axis center)
        9. (C)ircle 004 Quarter. (axis lower left)
        10. (C)ircle 005 Half. (axis lower middle)
        11. (C)ircle 006. (axis center)
        12. (C)ircle 006 Half. (axis lower middle)
        13. (C)ircle 007 Quarter. (axis lower left)
        14. (C)ircle 008 Half. (axis lower middle)
        15. (C)ircle 008 Quarter. (axis lower left)
        16. (C)ircle 009 Half. (axis lower middle)
        17. (H)orizontal 000. (axis upperLeft)
        18. (J)ump 000 (D)own. (axis lower right)
        19. (J)ump 000 (U)p. (axis lower left)
        20. (J)ump 001 (D)own. (axis lower right)
        21. (J)ump 001 (U)p. (axis lower left)
        22. (J)ump 002 (D)own. (axis lower right)
        23. (J)ump 002 (U)p. (axis lower left)
        24. (L)oop 000. (axis track end)
        25. (L)oop 001. (axis track end)
        26. (L)oop 002. (axis track middle)
        27. (S)pecial track 000. (axis track left)
        28. (S)pecial track 001. (axis track left)
        29. (S)pecial track 002. (axis track left)
        30. Start 000. (axis track right)
        31. (W)ave 000 (D)own. (axis lower left)
        32. (W)ave 000 (U)p. (axis lower left)
        33. (W)ave 001 (D)own. (axis lower left)
        34. (W)ave 001 (U)p. (axis lower left)
        35. (W)ave 002 (D)own. (axis lower left)
        36. (W)ave 002 (U)p. (axis lower left)
        37. StuntFan 000. (axis center)
        38. SawDisc 000. (axis center)
        39. Square 000. (axis center)
        40. Triangle 000. (axis center)

6. Slope modules. (V1.03)
     -The slope module now allows you to assemble or connect terrain of different heights.
     -There are two types of slope modules: grass and rock.
     -Grass and rocks can be mixed (blended).
     -A mini-game prefab has been added along with the slope module (downhill).
     

     6-1. Slope.
          6-1-1. Features
               -A slope of about 22 degrees is formed (50 meters on the floor and 20 meters in height).
               -The road can be connected with the ramp module included in the slope.(note. Lamps are connected only in a straight line.)
               -In addition to the lamp module that connects the road, a bridge lamp is also included.(note. Bridges are connected only in a straight line.)
               -Optimized collision mesh-The collision surface is designed based on 10x10. (note : Polygons less than 10x10 can be buried.)

          6-1-2. Slope modules List.
               -18 Slope modules + 2 Bridge. / Two types of slopes. (grass, cliff)

               1. Straight Bridge. (Bg_DHBridgeA00B)
               2. Straight Ramp Bridge. (Bg_DHBridgeA00R00)
               3. Corner Slope-Grass. (Bg_DHSlopeA00C00)
               4. Corner Slope-Rock. (Bg_DHSlopeA00C01)
               5. Corner Negative Resist-Grass. (Bg_DHSlopeA00CN00)
               6. DiagonalA-Grass. (Bg_DHSlopeA00Da00)
               7. DiagonalB-Grass. (Bg_DHSlopeA00Db00)
               8. Side Slope-Grass. (Bg_DHSlopeA00I00)
               9. Side Slope-Rock. (Bg_DHSlopeA00I01)
               10. Side Slope-Rock&Grass. (Bg_DHSlopeA00I02)
               11. Side Slope-Rock&Grass. (Bg_DHSlopeA00I03)
               12. Side Slope-Rock&Grass. (Bg_DHSlopeA00I04)
               13. Side Slope Ramp-Grass. (Bg_DHSlopeA00R00)
               14. Side Slope Ramp-Rock. (Bg_DHSlopeA00R01)
               15. Side Slope Ramp2-Grass. (Bg_DHSlopeA00R02)
               16. Side Slope Ramp End-Grass. (Bg_DHSlopeA00RE00)
               17. Side Slope Ramp End2-Grass. (Bg_DHSlopeA00RE01)
               18. Side Special-Rock. (Bg_DHSlopeA00S00)
               19. Side SlopeB1-Grass. (Bg_DHSlopeA01I00)
               20. Side SlopeB2-Grass. (Bg_DHSlopeA01I01)

     6-2. Downhill
          -Downhill preview scene : Press Play in the editor to enjoy the ride.
          -Slide through a dangerous, broken ramp. ("A"-left, "D"-right. / Do not go off the slope.)
          -Jump on a broken ramp. ("Space Bar" Press-wheelie, "Space Bar" up-jump / If you're lucky, you can get gems in the air.)
          -It will end when you reach all the slopes to the finish line...  but, it's not game, just preview. Enjoy!
          *note) The prefab used for downhill is prepared for a broken road.

          6-2-1. Downhill Course List.
               
               1. Straight Ramp Course1. Bg_DHCourseAI000
               2. Straight Ramp Course2. Bg_DHCourseAI001
               3. Straight Ramp Course3. Bg_DHCourseAI002
               4. (J) shape Ramp Course. Bg_DHCourseAJB000
               5. (L) shape Ramp Course. Bg_DHCourseALB000
               6. (Z) shape Ramp Course. Bg_DHCourseAZB000
               7. (Z) shape Broken Ramp Course. Bg_DHCourseAZB001
               8. Jump Rock1. Bg_DHJumpA001
               9. Jump Rock2. Bg_DHJumpA002
               10. Short Ramp-Low. Bg_DHJumpA003
               11. Short Ramp-Middle. Bg_DHJumpA004
               12. Short Ramp-High. Bg_DHJumpA005
               13. Jump Butte Diagonal. Bg_DHJumpA006
               14. Long Jump Butte. Bg_DHJumpA007
               15. Short Jump Butte. Bg_DHJumpA008

     6-3. V1.03 Additional environmental prefabs
               Decal x2. (Green, Khaki)
               Flower x5. (Blue, Green, Orange, Pink, Mix)
               Gem x1.
               Rock x3.
               Rock Pile x1.
               Tree x3.
               Dummy Bike x1.

7. Infinite runner. (V1.04)

     7-1. Tracks
               Added tracks available for both terrain and endless runners.

     7-2. modules
               A module that can be used immediately by drag and drop has been added.
               There is a bend module, so if you can make a pass code, you can make a bend too.

     7-3. scripts
               Infinite runner demo game script that can be played by swiping the screen has been added.
               The infinite runner script was implemented with only straight roads.

     7-4. scenes
               The playable scene has been added by pressing the play button on the editor.

8. A tropical beach theme was added. (V1.05)
               
     Tropical beach themed grounds, tracks, slopes, and props have been added.
     
     8-1. grounds.
               1 beach ground + 3 transition grounds.
               The transition block connects the forest and the beach.

     8-2. tracks.
               14 beach tracks.

     8-3. slopes.
               5 beach slopes + 2 transition slopes
               The transition block connects the forest and the beach.

     8-4. props.
               8 tropical plants (+1 cactus for assembly), boat, lifeguard station, dummy mountain, water foam

9. Textured track
    The textured track can be seen in the example scene (Display_TrackTextured).
    Textured tracks can use textures, Use textures in your tracks because meshes support uv coordinates.
    By changing the texture, you can create a variety of tracks.
    The example scene contains examples using grass, sand, stone, and snow textures.

    9-1. Path
    -Assets\LowPolyTerrain-Track\Meshs\UV
        The path contains the FBX.

    -Assets\LowPolyTerrain-Track\Models\Track\UV
        That path contains the default prefab with the material applied.

    -Assets\LowPolyTerrain-Track\Textures
        Contains example textures for meshes that support UVs.
        (Bg_Tx_Blue, Bg_Tx_Gray, Bg_Tx_Green, Bg_Tx_Yellow)

    9-2. Notes on using textures
        -A texture with a raised pattern requires caution when using it.
        -Please refer to the texture presented in the package.(Assets\LowPolyTerrain-Track\Textures\Bg_Tx_Blue, Bg_Tx_Gray, Bg_Tx_Green, Bg_Tx_Yellow)

    

10. User request track
    -three-way track
        You can assemble a three-way street.(Added both tracks with and without uv)
    -Straight hill track
        Added hill track consisting only of straight roads.(Added both tracks with and without uv)
        The hill track can also be used as a jumping off point.(Bg_54x54W10A00OverBreake)
        The hill track is adjustable in length.(Bg_54x54W10A00OverCustom)
    -30 degree / 60 degree bend track
        30 degree/60 degree bend straight road(Added only with tracks without uv, excluding tracks with uv)
        The 3060 track is not compatible with the module. It was added by user request and is used for special purposes only.
        *Meshs (Assets\LowPolyTerrain-Track\Meshs\3060)
        *Prefabs (Assets\LowPolyTerrain-Track\Models\Track\3060)
        *Scene (Assets\LowPolyTerrain-Track\Scenes\Display_3060Track)


*If you have any questions or suggestions about the assets, please contact me.(id3644@gmail.com)
Thank you for your purchase.
