using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class GenerateScene : MonoBehaviour
{
    public int checkpointNum;
    GameObject[] objects;
    GameObject[] hex;
    GameObject finalCheckpoint, coin;
    public static string[] hex_name = {
        "grass",
        "sand",
        "stone",
        "water",
        "speedUp",
        "speedDown",
        "jump",
        "checkpoint",
        "start"
    };
    string[] pattern_object = {
        "RockCliff-03", //6*4
        "RockCliff-01", //4*4
        "Tree-04", //3*4
        "Tree-02", //2*2
        "Tree-05", //2*3
        "TreeBirch-01", //2*2
        "TreeBirch-03", //2*2
        "TreeBirch-05", //2*3
        //"TreeBirch-06", //2*2
        "TreeDead-04", //2*2
        "RoadSign-01", //2*2
        "ShrubTall-01", //2*2
        "PondReed-02", //1*1
        "Tree_Sml-01", //1*1
        //"Tree_Sml-02", //1*1
        "Tree_Sml-03", //1*1
        //"Tree_Sml-04", //1*1
    };
    int[] pattern_object_num = {
        2, //"RockCliff-03", 6*4
        2, //"RockCliff-01", 4*4
        1, //"Tree-04", 3*4
        1, //"Tree-02", 2*2
        1, //"Tree-05", 2*3
        1, //"TreeBirch-01", 2*2
        1, //"TreeBirch-03", 2*2
        1, //"TreeBirch-05", 2*3
        //1, //"TreeBirch-06", 2*2
        1, //"TreeDead-04", 2*2
        1, //"RoadSign-01", 2*2
        1, //"ShrubTall-01", 2*2
        1, //"PondReed-02", 1*1
        1, //"Tree_Sml-01", 1*1
        //1, //"Tree_Sml-02", 1*1
        1, //"Tree_Sml-03", 1*1
        //1, //"Tree_Sml-04", 1*1
    };

    string[] small_object = {
        "Tree-01", //4*5
        "Tree-03", //5*5
        "Tree-04", //3*4
        "Tree-05", //2*3
        "RoadFenceWood_Damaged-01", //3*3
        "RoadFenceWood_Lrg-01", //2*4
        "RoadFenceWood_Sml-03", //2*1
        "RockMed-02", //2*1
        "ShrubTall-01", //2*2
        "ShrubTall-02", //1*1
        "TreeLog_B-02", //2*1
        "TreeLog_B-03", //2*2
        "TreeStump_A-02", //2*2
        "TreeStump_B-03", //2*2
        "RoadSign-01", //2*2
        "RoadLamp-01", //1*1
        "PondReed-02", //1*1
        /*"RockSml-02", //1*1
        "ShrubShort-02", //1*1
        "Grass-01", //1*1
        "Grass-02", //1*1 */
    };

    int[] small_object_num = {
        15, //"Tree-01", 4*5
        15, //"Tree-03", 5*5
        15, //"Tree-04", 3*4
        15, //"Tree-05", 2*3
        5, //"RoadFenceWood_Damaged-01", 3*3
        5, //"RoadFenceWood_Lrg-01", 2*4
        5, //"RoadFenceWood_Sml-03", 2*1
        30, //"RockMed-02", 2*1
        25,//"ShrubTall-01", 2*2
        30, //"ShrubTall-02", 1*1
        30, //"TreeLog_B-02", 2*1
        30, //"TreeLog_B-03", 2*2
        7, //"TreeStump_A-02", 2*2
        7, //"TreeStump_B-03", 2*2
        6, //"RoadSign-01", 2*2
        7, //"RoadLamp-01", 1*1
        7, //"PondReed-02", 1*1
        /*
        50, //"RockSml-02", 1*1
        50, //"ShrubShort-02", 1*1
        50, //"Grass-01", 1*1
        50, //"Grass-02", 1*1 */
    };

    string[] small_object_fill = {
        "RockSml-02", //1*1
        //"ShrubShort-02", //1*1
        "Grass-01", //1*1
        "Grass-02", //1*1
    };

    string[] large_rock = {
        "RockCliff-0203", //30*18
        "RockCliff-03", //26*15
        "RockCliff-03-1", //17*22
        "RockCliff-02", //17*17
        "RockCliff-01" //13*12
    };

    int[] large_rock_num = {
        1, //"RockCliff-0203", 30*18
        1, //"RockCliff-03", 26*15
        1, //"RockCliff-03-1", 17*22
        1, //"RockCliff-02", 17*17
        1, //"RockCliff-01", 13*12
    };

    string[] large_object = {
        "Oak-02", //12*10
        //"Pond_Boardwalk-02", //8*5
        "TreeBirch-05", //8*8
        "TreeBirch-02", //7*8
        "Tree-05", //10*12
        "Tree-03", //9*11
        "Tree-02", //7*10
        "Tree-04", //7*10
        "Tree-01", //7*9
        "TreeBirch-01", //4*4
        "TreeDead-01", //5*6
        "TreeDead-02", //4*3
        "Birch-03", //8*8
    };

    int[] large_object_num = {
        3, //"Oak-02", 12*10
        //2, //"Pond_Boardwalk-02", 8*5
        2, //"TreeBirch-05", 8*8
        2, //"TreeBirch-02", 7*8
        2, //"Tree-05", 10*12
        2, //"Tree-03", 9*11
        2, //"Tree-02", 7*10
        2, //"Tree-04", 7*10
        2, //"Tree-01", 7*9
        1, //"TreeBirch-01", 4*4
        2, //"TreeDead-01", 5*6
        2, //"TreeDead-02", 4*3
        1 //"Birch-03", 8*8
    };

    void Awake()
    {
        LoadHexagonObjects();
        Scene.Initialize();

        PlaceStartHexagons();
        PlaceEndHexagons();

        // place checkpoints
        Scene.GetCheckpointPosValue();
        PlaceCheckpointHexagonPatterns(Scene.checkpointPos.Length, Scene.checkpointPos, 3);

        PlaceAllHexagonPatternsInArray(pattern_object, pattern_object_num, "Prefabs/Scene/Hex/", 20);

        ConnectAllHexagons();

        // Random Area
        //GenerateRandomArea(2, 10, 15);
        
        PlaceAllSceneInArray(large_rock, large_rock_num, "Prefabs/Scene/Large/", 20);
        PlaceAllSceneInArray(large_object, large_object_num, "Prefabs/Scene/Large/", 10);
        PlaceAllSceneInArray(small_object, small_object_num, "Prefabs/Scene/Small/", 10);
        
        
        /*for(int i=0; i<50; i++)
            for(int j=0; j<50; j++)
            {
                PlaceHexagon(i, j, sand);
            }*/
        FillRestArea(small_object_fill, "Prefabs/Scene/Small/", 3);
    }

    void LoadHexagonObjects()
    {
        string dir = "Prefabs/Hex/";
        hex = new GameObject[hex_name.Length];
        for (int i=0; i<hex_name.Length; i++)
        {
            hex[i] = (GameObject)Resources.Load(dir+hex_name[i]);
        }

        finalCheckpoint = (GameObject)Resources.Load("Prefabs/finalCheckpoint");
        coin = (GameObject)Resources.Load("Prefabs/coin");
    }

    void PlaceStartHexagons()
    {

        GenerateRandomArea(4, 8, 1, true, 0, 0);
    }

    void PlaceEndHexagons()
    {
        PlaceHexagonPattern(3, 3, finalCheckpoint, 100, 1, 3, 5, 3, true, Scene.areaLength-8, Scene.areaLength-8);
    }

    void FillRestArea(string[] name, string dir, int ratio)
    {
        GameObject[] objects = new GameObject[name.Length];

        for(int i=0; i<name.Length; i++)
            objects[i] = (GameObject)Resources.Load(dir+name[i]);

        for(int i=0; i<Scene.areaLength; i++)
            for(int j=0; j<Scene.areaLength; j++)
            {
                int val = Random.Range(0, name.Length*ratio);
                if (val < name.Length)
                    PlaceHexagon(i, j, objects[val], false);
            }
    }

    void GenerateRandomArea(int lower_bound, int upper_bound, int times, bool use_xz=false, int static_x=-1, int static_z=-1)
    {
        int sample_count, x, z, height, width;
        for (int i=0; i<times; i++)
        {
            height = Random.Range(lower_bound, upper_bound+1);
            width = Random.Range(lower_bound, upper_bound+1);
            sample_count = 0;
            while(true) {
                sample_count += 1;
                if(sample_count >= 5) break;
                if (use_xz && static_x != -1 && static_z != -1) {
                    x = static_x;
                    z = static_z;
                }
                else {
                    x = Random.Range(0, Scene.areaLength-height);
                    z = Random.Range(0, Scene.areaLength-width);
                }

                Scene.hexNum hexN = new Scene.hexNum {num_x=x, num_z=z};
                Scene.hexNum[] area = Hexagon.GetRandomArea(hexN, height, width);
                if(Scene.JudgeAreaOccupancy(area))
                {
                    //Scene.SetAreaOccupancy(area); -> Occupancy set in PlaceHexagon
                    PlaceHexagon(area, hex[1]); //sand
                    break;
                }
            }
        }
    }

    void ConnectAllHexagons()
    {
        Scene.SortHexagonCenter();

        Scene.hexNum[] line;
        Scene.hexNum start, end;

        //Get the first element in hexCenter
        start = new Scene.hexNum{
            num_x = 0,
            num_z = 0
        };
        end = Scene.GetNextHexagonCenter(0, -1);
        line = Hexagon.GetLineBetweenPointsOffset(start, end);
        Utils.Add(ref line, start);
        PlaceHexagon(line, hex[1]); //sand

        for(int k=0; k<9; k++)
        {
            for (int i=0; i<Scene.hexCenter[k].Length;i++)
            {
                start = Scene.hexCenter[k][i];
                end = Scene.GetNextHexagonCenter(k, i);
                if (end.num_x == -1) break; // reach end of center -> change to goal later

                line = Hexagon.GetLineBetweenPointsOffset(start, end);
                PlaceHexagon(line, hex[1]); //sand


                int level = 1;
                //int distance = Hexagon.DistanceBetweenPointsOffset(start, end);
                //if (distance >= 64) level = 3;
                //if (distance >= 32) level = 2;
                //else if (distance >= 8) level = 1;

                line = Hexagon.GetNoisyEdgesBetweenPointsOffset(start, end, level, 0.4f);
                PlaceHexagon(line, hex[2]); //stone
                
                line = Hexagon.GetTwistedEdgesBetweenPointsOffset(start, end, 1f);
                PlaceHexagon(line, hex[3]); //blue
                
            }
        }
    }

    void PlaceHexagon(Scene.hexNum[] array, GameObject obj)
    {
        for (int j=0; j<array.Length; j++)
            PlaceHexagon(array[j].num_x, array[j].num_z, obj);
    }

    void PlaceHexagon(int x, int z, GameObject obj, bool place_coin=true)
    {
        int num;
        Scene.hexAxis hexA;
        Vector3 pos;

        num = Scene.GetOneDimensionVal(x, z);
        if (!Scene.mapOccupied[num])
        {
            hexA = Hexagon.NumToAxis(x, z);
            pos.x = hexA.axis_x;
            pos.z = hexA.axis_z;
            pos.y = 0;

            if (pos.x==0&&pos.z==0)
            {
                obj = hex[8];
            }

            else if (place_coin && Random.Range(0f, 1f) < 0.05 && !(pos.x==0&&pos.z==0))
            {
                obj = hex[4+Random.Range(0, 3)];
            }

            Scene.map[num] = Instantiate(obj, pos, Quaternion.identity, transform);
            Scene.mapOccupied[num] = true;

            // place coin
            if (place_coin && Random.Range(0f, 1f) < 0.1 && !(pos.x==0&&pos.z==0))
                {
                    pos.y = 0.71f;
                    Instantiate(coin, pos, Quaternion.identity, transform);
                }
        }
    }

    GameObject PlaceSceneObject(int x, int z, int height, int width, GameObject obj)
    {
        int num;
        Scene.hexAxis hexA;
        Vector3 pos;

        Scene.SetRectAreaOccupancy(x, z, height, width);
        hexA = Scene.GetRectAreaCenter(x, z, height, width);
        pos.x = hexA.axis_x - obj.GetComponent<Space>().bias_x;
        pos.z = hexA.axis_z - obj.GetComponent<Space>().bias_z;
        pos.y = 0;
        num = Scene.GetOneDimensionVal(x, z);
        Scene.map[num] = Instantiate(obj, pos, Quaternion.identity, transform);
        return Scene.map[num];
    }

    void PlaceAllSceneInArray(string[] name, int[] num, string dir, int count)
    {
        GameObject[] objects = new GameObject[name.Length];
        int height, width, sample_count, place_count, x, z;
        for (int i=0; i<name.Length; i++)
        {
            objects[i] = (GameObject)Resources.Load(dir+name[i]);
            height = objects[i].GetComponent<Space>().height;
            width = objects[i].GetComponent<Space>().width;
            sample_count = 0;
            place_count = 0;

            while(true) {
                sample_count += 1;
                if(sample_count >= count) break;

                x = Random.Range(0, Scene.areaLength-height);
                z = Random.Range(0, Scene.areaLength-width);
                //x = Random.Range(0, 50-height);
                //z = Random.Range(0, 50-width);

                if(Scene.JudgeRectAreaOccupancy(x, z, height, width))
                {
                    PlaceSceneObject(x, z, height, width, objects[i]);
                    place_count += 1;
                    if (place_count == num[i]) break;
                }
            }
        }
    }

    void PlaceHexagonPattern(int height, int width, GameObject obj, int count, int place_num, int lower_bound, int upper_bound, int edge_distance, bool use_xz=false, int static_x=-1, int static_z=-1, bool checkpoint=false, int num=-1, int total=-1)
    {
        GameObject obj_new;
        // use_xz = true: for placing checkpoint
        int sample_count = 0, place_count = 0;
        int x, z, x1, z1;
        while(true) {
            sample_count += 1;
            if(sample_count >= count) break;

            if (use_xz && static_x != -1 && static_z != -1) {
                x = static_x;
                z = static_z;
            }
            else {
                x = Random.Range(0, Scene.areaLength-height);
                z = Random.Range(0, Scene.areaLength-width);
                //x = Random.Range(0, 50-height);
                //z = Random.Range(0, 50-width);
            }

            int radius = Mathf.FloorToInt(Mathf.Max(height/2f, width/2f))+1+Random.Range(lower_bound, upper_bound+1); //3 -> function(width/height)

            Scene.hexNum hexN = new Scene.hexNum {num_x=x, num_z=z};
            Scene.hexNum[] ring = Hexagon.GetSpiralRing(hexN, radius);
            if (ring == null) continue;

            if(Scene.JudgeAreaOccupancy(ring))
            {
                Scene.AddHexagonCenter(hexN);
                int min_x = ring[6*radius-7].num_x;
                int max_x = ring[3*radius-4].num_x;
                int min_z = ring[radius-2].num_z;
                int max_z = ring[4*radius-5].num_z;

                while(true) {
                    x1 = Random.Range(min_x+edge_distance, max_x-height-edge_distance+1);
                    z1 = Random.Range(min_z+edge_distance, max_z-width-edge_distance+1);
                    if (Scene.JudgeRectInRing(x1, z1, height, width, ring))
                    {
                        //if (use_xz)
                            //PlaceHexagon(x1, z1, obj);
                        //else
                        obj_new = PlaceSceneObject(x1, z1, height, width, obj);
                        if (checkpoint && num!=-1 && total!=-1)
                        {
                            obj_new.GetComponent<Checkpoint>().num = num;
                            obj_new.GetComponent<Checkpoint>().total = total;
                        }
                        PlaceHexagon(ring, hex[0]); //grass
                        place_count += 1;
                        break;
                    }
                }

                if (place_count == place_num) break;
            }
            
        }
    }

    void PlaceAllHexagonPatternsInArray(string[] name, int[] num, string dir, int count)
    {
        GameObject[] objects = new GameObject[name.Length];
        int height, width;
        for (int i=0; i<name.Length; i++)
        {
            objects[i] = (GameObject)Resources.Load(dir+name[i]);
            height = objects[i].GetComponent<Space>().height;
            width = objects[i].GetComponent<Space>().width;

            PlaceHexagonPattern(height, width, objects[i], count, num[i], 2, 3, 2);
        }
    }

    void PlaceCheckpointHexagonPatterns(int place_num, Scene.hexNum[] pos, int count)
    {
        GameObject obj = (GameObject)Resources.Load("Prefabs/Hex/checkpoint");
        int height = obj.GetComponent<Space>().height;
        int width = obj.GetComponent<Space>().width;
        for (int i=0; i<place_num; i++)
            PlaceHexagonPattern(height, width, obj, count, 1, 2, 3, 2, true, pos[i].num_x, pos[i].num_z, true, i, place_num);
    }
}
