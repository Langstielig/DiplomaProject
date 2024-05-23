using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//x - length
//y - height
//z - width

public class GenerateBSPMap : MonoBehaviour
{
    //[SerializeField] private GameObject ground1;
    //[SerializeField] private GameObject wall1;
    //[SerializeField] private GameObject border;
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject lowerWall;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject upperWall;
    //[SerializeField] private GameObject light;
    [SerializeField] private GameObject femaleCharacter;
    [SerializeField] private GameObject maleCharacter;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject ladder;

    private int countOfRooms = 1;
    private GameObject emptyRooms;
    private GameObject emptyHalls;
    //GameObject emptyLeafs = new GameObject("Leafs");
    private GameObject emptyMainCharacter;
    private GameObject emptyEnemies;
    private GameObject emptyChests;
    private GameObject emptyLadder;
    //private int test = 0;

    [SerializeField] private int countOfEnemies;
    private int countOfChests = 4;

    void Start()
    {
        // Hall hall = new Hall(0, 0, 8, false, true);
        // hall.HallDrawing(ground1, wall1);

        // Room room = new Room(0, 0, 8, 8, ref countOfRooms);
        // room.RoomDrawing(ground1, wall1);

        // Room room2 = new Room(10, 10, 5, 5, ref countOfRooms);
        // room2.RoomDrawing(ground1, wall1);

        //Hall hall1 = new Hall(11, 2, 8, true, 1, 2, ref emptyHalls);
        //hall.HallDrawing(ground1, wall1);

        //Hall hall2 = new Hall(3, 8, 8, false, 1, 2, ref emptyHalls);

        //Room room = new Room(0, 0, 11, 8, 1, ref emptyRooms, hall1, hall2);
        //room.RoomDrawing(ground1, wall1);

        emptyRooms = new GameObject("Rooms");
        emptyHalls = new GameObject("Halls");
        emptyMainCharacter = new GameObject("MainCharacter");
        emptyEnemies = new GameObject("Enemies");
        emptyChests = new GameObject("Chests");
        emptyLadder = new GameObject("Ladder");

        Leaf leaf = new Leaf(0, 0, 100, 100);
        leaf.Split();
        leaf.GenerateRooms(ref countOfRooms, ref emptyRooms, ref emptyHalls);
        leaf.GenerateHallsBetweenSiblings(ref emptyHalls);

        Leaf[] leafsWithoutHalls = new Leaf[countOfRooms - 1];
        for (int i = 0; i < countOfRooms - 1; i++)
            leafsWithoutHalls[i] = null;

        leaf.FindLeafsWithoutHalls(ref leafsWithoutHalls);
        int index = 0;
        while (leafsWithoutHalls[index] != null)
        {
            Leaf newLeaf = leaf.FindCloseLeaf(leafsWithoutHalls[index]);
            if(newLeaf != null)
            {
                leaf.GenerateRemainingHall(leafsWithoutHalls[index], newLeaf, ref emptyHalls);
            }
            index++;
        }

        leaf.DrawMap(ground, lowerWall, wall, upperWall);
        leaf.GenerateMainCharacter(femaleCharacter, maleCharacter, ref emptyMainCharacter);
        leaf.GenerateEnemies(enemy, countOfRooms - 1, countOfEnemies, ref emptyEnemies);
        leaf.GenerateChests(chest, countOfRooms - 1, countOfChests, ref emptyChests);
        leaf.GenerateLadder(ladder, ref emptyLadder);

        //Leaf leaf = new Leaf(0, 0, 50, 50);
        ////leaf.LeafDrawing(border, ref test, ref emptyLeafs);
        //leaf.Split();
        //leaf.GenerateRooms(ground1, wall1, border, ref countOfRooms, ref emptyRooms, ref emptyHalls, ref emptyLeafs);

    }

    void Update()
    {
        CheckTabButton(ref emptyMainCharacter, ref emptyEnemies, ref emptyChests);
    }

    private void CheckTabButton(ref GameObject emptyMainCharacter, ref GameObject emptyEnemies, ref GameObject emptyChests)
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = emptyEnemies.activeSelf;
            emptyEnemies.SetActive(!isActive);
            isActive = emptyChests.activeSelf;
            emptyChests.SetActive(!isActive);
        }
    }

    public static bool RandomBoolean()
    {
        ///<summary>
        ///returns random boolean value
        ///</summary>
        if(Random.value > 0.5)
        {
            return true;
        }
        else if(Random.value < 0.5)
        {
            return false;
        }
        else
        {
            return RandomBoolean();
        }
    }

    class Leaf
    {
        private const int minLeafSize = 15;

        public int x0;
        public int z0;

        public int width;
        public int length;

        public Leaf leftChild;
        public Leaf rightChild;

        public bool isHorizontalSplitting;
        public bool isSplitted;

        public Room room;

        /// <summary>
        /// constructor without parameters
        /// </summary>
        public Leaf()
        {
            x0 = 0;
            z0 = 0;

            width = 0;
            length = 0;

            leftChild = null;
            rightChild = null;

            isHorizontalSplitting = true;
            isSplitted = false;

            room = null;
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        /// <param name="_x">x coordinate of lower left corner</param>
        /// <param name="_z">z coordinate of lower left corner</param>
        /// <param name="l">length (size on x coordinate)</param>
        /// <param name="w">width (size on z coordinate)</param>
        public Leaf(int _x, int _z, int l, int w)
        {
            x0 = _x;
            z0 = _z;

            length = l;
            width = w;

            leftChild = null;
            rightChild = null;

            isHorizontalSplitting = true;
            isSplitted = false;

            room = null;
        }

        /// <summary>
        /// function for splitting the space
        /// </summary>
        public void Split()
        {
            //Debug.Log("Split leaf: " + x0.ToString() + ", " + z0.ToString() + ", " + length.ToString() + ", " + width.ToString());
            if(leftChild == null && rightChild == null)
            {
                bool horizontalSplitting;

                if(width > length && (double)width / (double)length >= 1.25)
                {
                    horizontalSplitting = true;
                    //Debug.Log("width > length");
                }
                else if(length > width && (double)length / (double)width >= 1.25)
                {
                    horizontalSplitting = false;
                    //Debug.Log("length > width");
                }
                else
                {
                    horizontalSplitting = RandomBoolean();
                    //horizontalSplitting = false;
                    //Debug.Log("random");
                }
                //Debug.Log("HorizontalSplitting: " + horizontalSplitting);

                int max;
                if(horizontalSplitting)
                {
                    max = width - minLeafSize;
                }
                else
                {
                    max = length - minLeafSize;
                }

                if(max > minLeafSize)
                {
                    int splitPlace = Random.Range(minLeafSize, max);
                    //Debug.Log("SplitPlace: " + splitPlace);
                    if(horizontalSplitting)
                    {
                        isHorizontalSplitting = true;
                        isSplitted = true;
                        leftChild = new Leaf(x0, z0, length, splitPlace);
                        //Debug.Log("leftChild: " + x0.ToString() + ", " + z0.ToString() + ", " + length.ToString() + ", " + splitPlace.ToString());
                        rightChild = new Leaf(x0, z0 + splitPlace, length, width - splitPlace);
                        //Debug.Log("rightChild: " + x0.ToString() + ", " + (z0 + splitPlace).ToString() + ", " + length.ToString() + ", " + (width - splitPlace).ToString());
                    }
                    else
                    {
                        isHorizontalSplitting = false;
                        isSplitted = true;
                        leftChild = new Leaf(x0, z0, splitPlace, width);
                        //Debug.Log("leftChild: " + x0.ToString() + ", " + z0.ToString() + ", " + splitPlace.ToString() + ", " + width.ToString());
                        rightChild = new Leaf(x0 + splitPlace, z0, length - splitPlace, width);
                        //Debug.Log("rightChild: " + (x0 + splitPlace).ToString() + ", " + z0.ToString() + ", " + (length - splitPlace).ToString() + ", " + width.ToString());
                    }
                }
                else
                {
                    //Debug.Log("cant split this leaf because max <= minLeafSize");
                }
                if(leftChild != null)
                {
                    leftChild.Split();
                }
                if(rightChild != null)
                {
                    rightChild.Split();
                }
            }
            else
            {
                //Debug.Log("cant split this leaf");
            }
        }

        /// <summary>
        /// function for generating rooms in splitted space
        /// </summary>
        /// <param name="floor">floor prefab</param>
        /// <param name="wall">wall prefab</param>
        /// <param name="border">border prefab</param>
        /// <param name="roomNumber">parameter for counting generated rooms</param>
        /// <param name="emptyRooms">main empty GameObject for hierarchy for storing all generated rooms</param>
        /// <param name="emptyHalls">main empty GameObject for hierarchy for storing all generated halls</param>
        /// <param name="emptyLeafs">main empty GameObject for hierarchy for storing all generated leafs (parts of all space)</param>
        public void GenerateRooms(ref int roomNumber,
                                  ref GameObject emptyRooms, ref GameObject emptyHalls)
        //, ref GameObject emptyLeafs, GameObject border) //��� ����� ����� ������ ������ ���������, ������ ��� ����� �� ����� ����� ��������
        {
            if (leftChild != null || rightChild != null)
            {
                if(leftChild != null)
                {
                    leftChild.GenerateRooms(ref roomNumber, ref emptyRooms, ref emptyHalls);
                }
                if(rightChild != null)
                {
                    rightChild.GenerateRooms(ref roomNumber, ref emptyRooms, ref emptyHalls);
                }
            }
            else
            {
                //LeafDrawing(border, ref roomNumber, ref emptyLeafs);

                //Debug.Log("create room for leaf: " + x0.ToString() + ", " + z0.ToString() + ", " + width.ToString() + ", " + length.ToString());
                int roomWidth = Random.Range(10, width - 2);
                int roomLength = Random.Range(10, length - 2);
                int roomPosX = Random.Range(1, length - roomLength - 1) + x0;
                int roomPosZ = Random.Range(1, width - roomWidth - 1) + z0;

                room = new Room(roomPosX, roomPosZ, roomLength, roomWidth, roomNumber, ref emptyRooms);
                //Debug.Log("room number: " + roomNumber);
                roomNumber++;
                //Debug.Log("generated room: " + roomPosX.ToString() + ", " + roomPosZ.ToString() + ", " + roomLength.ToString() + ", " + roomWidth.ToString());
                //room.RoomDrawing(floor, wall);
            }
        }

        public void GenerateHallsBetweenSiblings(ref GameObject emptyHalls)
        {
            if(leftChild != null && leftChild.room != null && rightChild != null && rightChild.room != null)
            {
                int min;
                int max;
                int xPosition;
                int zPosition;
                int lengthOfHall;
                bool isHorizontal;
                if (isHorizontalSplitting)
                {
                    if (leftChild.room.x0 > rightChild.room.x0)
                        min = leftChild.room.x0;
                    else
                        min = rightChild.room.x0;

                    if ((leftChild.room.x0 + leftChild.room.length) < (rightChild.room.x0 + rightChild.room.length))
                        max = leftChild.x0 + leftChild.room.length - 3;
                    else
                        max = rightChild.room.x0 + rightChild.room.length - 3;

                    if(max <= min)
                    {
                        if(leftChild.room.x0 < rightChild.room.x0)
                        {
                            leftChild.room.length += (min - max + 1);
                            max = leftChild.x0 + leftChild.room.length - 3;
                        }
                        else
                        {
                            rightChild.room.length += (min - max + 1);
                            max = rightChild.room.x0 + rightChild.room.length - 3;
                        }
                    }

                    xPosition = Random.Range(min, max);
                    zPosition = leftChild.room.z0 + leftChild.room.width;
                    lengthOfHall = rightChild.room.z0 - zPosition;
                    isHorizontal = false;
                }
                else
                {
                    if (leftChild.room.z0 > rightChild.room.z0)
                        min = leftChild.room.z0;
                    else
                        min = rightChild.room.z0;

                    if ((leftChild.room.z0 + leftChild.room.width) < (rightChild.room.z0 + rightChild.room.width))
                        max = leftChild.room.z0 + leftChild.room.width - 3;
                    else
                        max = rightChild.room.z0 + rightChild.room.width - 3;

                    if(max <= min)
                    {
                        if(leftChild.room.z0  < rightChild.room.z0)
                        {
                            leftChild.room.width += (min - max + 1);
                            max = leftChild.room.z0 + leftChild.room.width - 3;
                        }
                        else
                        {
                            rightChild.room.width += (min - max + 1);
                            max = rightChild.room.z0 + rightChild.room.width - 3;
                        }
                    }

                    xPosition = leftChild.room.x0 + leftChild.room.length;
                    zPosition = Random.Range(min, max);
                    lengthOfHall = rightChild.room.x0 - xPosition;
                    isHorizontal = true;
                }
                Hall newHall = new Hall(xPosition, zPosition, lengthOfHall, isHorizontal, leftChild.room.roomNumber, rightChild.room.roomNumber, ref emptyHalls);
                leftChild.room.AddRealHall(newHall);
                rightChild.room.AddImaginaryHall(newHall);
            }
            else
            {
                if (leftChild != null)
                    leftChild.GenerateHallsBetweenSiblings(ref emptyHalls);
                if (rightChild != null)
                    rightChild.GenerateHallsBetweenSiblings(ref emptyHalls);
            }
        }

        public void FindLeafsWithoutHalls(ref Leaf[] leafsWithoutHalls)
        {
            if (room != null && room.upperHall == null && room.rightHall == null) 
            {
                int index = 0;
                while (leafsWithoutHalls[index] != null) index++;
                leafsWithoutHalls[index] = this;
            }
            else
            {
                if(leftChild != null)
                    leftChild.FindLeafsWithoutHalls(ref leafsWithoutHalls);
                if (rightChild != null)
                    rightChild.FindLeafsWithoutHalls(ref leafsWithoutHalls);
            }
        }

        public void GetLeafsAsList(ref Leaf[] leafs)
        {
            if(room != null)
            {
                int index = 0;
                while (leafs[index] != null) index++;
                leafs[index] = this;
            }
            else
            {
                if(leftChild != null)
                {
                    leftChild.GetLeafsAsList(ref leafs);
                }
                if(rightChild != null)
                {
                    rightChild.GetLeafsAsList(ref leafs);
                }
            }
        }

        public Leaf FindCloseLeaf(Leaf currentLeaf)
        {
            int middleOfRoomX = (2 * currentLeaf.x0 + currentLeaf.length) / 2;
            int middleOfRoomZ = (2 * currentLeaf.z0 + currentLeaf.width) / 2;
            if(room != null && 
               !((room.upperHall != null && room.upperHall == currentLeaf.room.lowerHall) || 
               (room.rightHall != null && room.rightHall == currentLeaf.room.leftHall)) &&
               //room.upperHall == null && room.rightHall == null &&
               (((currentLeaf.x0 + currentLeaf.length) == x0 && middleOfRoomZ > z0 && middleOfRoomZ < (z0 + width)) || //��������� ������� �� �������� � ��������� �� ����������
               ((currentLeaf.z0 + currentLeaf.width) == z0 && middleOfRoomX > x0 && middleOfRoomX < (x0 + length))))
            {
                return this;
            }
            else
            {
                Leaf leftLeaf = null;
                Leaf rightLeaf = null;
                if (leftChild != null)
                {
                    leftLeaf = leftChild.FindCloseLeaf(currentLeaf);
                }
                if (rightChild != null)
                {
                    rightLeaf = rightChild.FindCloseLeaf(currentLeaf);
                }
                
                //if (rightLeaf != null)
                //    return rightLeaf;
                //else if (leftLeaf != null)
                //    return leftLeaf;
                //else
                //    return null;

                if (leftLeaf == null && rightLeaf == null)
                    return null;
                else if (leftLeaf == null)
                    return rightLeaf;
                else if (rightLeaf == null)
                    return leftLeaf;
                else if (RandomBoolean())
                    return rightLeaf;
                else
                    return leftLeaf;
            }
        }

        public void GenerateRemainingHall(Leaf leftLeaf, Leaf rightLeaf, ref GameObject emptyHalls)
        {
            if(leftLeaf.room != null && rightLeaf.room != null)
            {
                int min;
                int max;
                int xPosition;
                int zPosition;
                int lengthOfHall;
                bool isHorizontal;
                if ((leftLeaf.x0 + leftLeaf.length) == rightLeaf.x0)
                {
                    if (leftLeaf.room.z0 > rightLeaf.room.z0)
                        min = leftLeaf.room.z0;
                    else
                        min = rightLeaf.room.z0;

                    if ((leftLeaf.room.z0 + leftLeaf.room.width) < (rightLeaf.room.z0 + rightLeaf.room.width))
                        max = leftLeaf.room.z0 + leftLeaf.room.width - 3;
                    else
                        max = rightLeaf.room.z0 + rightLeaf.room.width - 3;

                    if (max <= min)
                    {
                        if (leftLeaf.room.z0 < rightLeaf.room.z0)
                        {
                            leftLeaf.room.width += (min - max + 1);
                            max = leftLeaf.room.z0 + leftLeaf.room.width - 3;
                        }
                        else
                        {
                            rightLeaf.room.width += (min - max + 1);
                            max = rightLeaf.room.z0 + rightLeaf.room.width - 3;
                        }
                    }

                    xPosition = leftLeaf.room.x0 + leftLeaf.room.length;
                    zPosition = Random.Range(min, max);
                    lengthOfHall = rightLeaf.room.x0 - xPosition;
                    isHorizontal = true;
                }
                else /*if ((leftLeaf.z0 + leftLeaf.width) == rightLeaf.z0)*/
                {
                    if (leftLeaf.room.x0 > rightLeaf.room.x0)
                        min = leftLeaf.room.x0;
                    else
                        min = rightLeaf.room.x0;

                    if ((leftLeaf.room.x0 + leftLeaf.room.length) < (rightLeaf.room.x0 + rightLeaf.room.length))
                        max = leftLeaf.x0 + leftLeaf.room.length - 3;
                    else
                        max = rightLeaf.room.x0 + rightLeaf.room.length - 3;

                    if (max <= min)
                    {
                        if (leftLeaf.room.x0 < rightLeaf.room.x0)
                        {
                            leftLeaf.room.length += (min - max + 1);
                            max = leftLeaf.x0 + leftLeaf.room.length - 3;
                        }
                        else
                        {
                            rightLeaf.room.length += (min - max + 1);
                            max = rightLeaf.room.x0 + rightLeaf.room.length - 3;
                        }
                    }

                    xPosition = Random.Range(min, max);
                    zPosition = leftLeaf.room.z0 + leftLeaf.room.width;
                    lengthOfHall = rightLeaf.room.z0 - zPosition;
                    isHorizontal = false;
                }
                Hall newHall = new Hall(xPosition, zPosition, lengthOfHall, isHorizontal, leftLeaf.room.roomNumber, rightLeaf.room.roomNumber, ref emptyHalls);
                leftLeaf.room.AddRealHall(newHall);
                rightLeaf.room.AddImaginaryHall(newHall);
            }
        }

        public Room GetRoom()
        {
            if (room != null)
                return room;
            else
            {
                Room leftRoom = null;
                Room rightRoom = null;

                if(leftChild != null)
                    leftRoom = leftChild.GetRoom();
                if(rightChild != null)
                    rightRoom = rightChild.GetRoom();

                if (leftRoom == null && rightRoom == null)
                    return null;
                else if (rightRoom == null)
                    return leftRoom;
                else if (leftRoom == null)
                    return rightRoom;
                else if(RandomBoolean())
                    return leftRoom;
                else
                    return rightRoom;
            }
        }

        public void DrawMap(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall)
        {
            if (room != null)
            {
                //LeafDrawing(border, ref roomNumber, ref emptyLeafs);
                room.RoomDrawing(floor, lowerWall, wall, upperWall);
            }
            else
            {
                if (leftChild != null)
                    leftChild.DrawMap(floor, lowerWall, wall, upperWall);
                if (rightChild != null)
                    rightChild.DrawMap(floor, lowerWall, wall, upperWall);
            }
        }

        public void GenerateMainCharacter(GameObject femaleCharacter, GameObject maleCharacter, 
                                          ref GameObject emptyMainCharacter)
        {
            if(room != null)
            {
                Vector3 characterPosition = room.GetRoomCenter();
                characterPosition.y = 0;
                GameObject mainCharacter;
                if (DataHolder.mainCharacter == "female")
                {
                    mainCharacter = Instantiate(femaleCharacter);
                }
                else
                {
                    mainCharacter = Instantiate(maleCharacter);
                }
                mainCharacter.transform.position = characterPosition;
                mainCharacter.transform.SetParent(emptyMainCharacter.transform);
            }
            else
            {
                if(leftChild != null)
                {
                    leftChild.GenerateMainCharacter(femaleCharacter, maleCharacter, ref emptyMainCharacter);
                }
            }
        }

        public void GenerateEnemies(GameObject enemy, int countOfLeafs, int countOfEnemies, ref GameObject emptyEnemies)
        {
            Leaf[] leafs = new Leaf[countOfLeafs];
            for (int i = 0; i < countOfLeafs; i++)
                leafs[i] = null;

            GetLeafsAsList(ref leafs);

            int step = countOfLeafs / countOfEnemies;
            for(int i = Random.Range(1, step - 1); i < countOfLeafs - 1; i += step)
            {
                if(countOfEnemies > 0)
                {
                    Vector3 enemyPosition = leafs[i].room.GetRoomCenter();
                    enemyPosition.y = 0;
                    GameObject newEnemy = Instantiate(enemy);
                    newEnemy.transform.position = enemyPosition;
                    newEnemy.transform.SetParent(emptyEnemies.transform);
                    countOfEnemies--;
                }
            }
        }

        public void GenerateChests(GameObject chest, int countOfLeafs, int countOfChests, ref GameObject emptyChests)
        {
            Leaf[] leafs = new Leaf[countOfLeafs];
            for (int i = 0; i < countOfLeafs; i++)
                leafs[i] = null;

            GetLeafsAsList(ref leafs);

            int step = countOfLeafs / countOfChests;
            for (int i = Random.Range(1, step - 1); i < countOfLeafs - 1; i += step)
            {
                if (countOfChests > 0)
                {
                    Vector3 chestPosition = leafs[i].room.GetRoomCenter();
                    chestPosition.y = 0;
                    GameObject newChest = Instantiate(chest);
                    newChest.transform.position = chestPosition;
                    newChest.transform.SetParent(emptyChests.transform);
                    countOfChests--;
                }
            }
        }

        public void GenerateLadder(GameObject ladder, ref GameObject emptyLadder)
        {
            if (room != null)
            {
                Vector3 ladderPosition = room.GetRoomCenterWall();
                ladderPosition.z -= 1;
                GameObject newLadder = Instantiate(ladder);
                newLadder.transform.position = ladderPosition;
                newLadder.transform.SetParent(emptyLadder.transform);
            }
            else
            {
                if (rightChild != null)
                {
                    rightChild.GenerateLadder(ladder, ref emptyLadder);
                }
            }
        }

        /// <summary>
        /// function for drawing borders of created leaf
        /// </summary>
        /// <param name="border">border prefab</param>
        /// <param name="leafNumber">parameter for counting leafs</param>
        /// <param name="emptyLeafs">main empty GameObject for hierarchy for storing all generated leafs (parts of all space</param>
        public void LeafDrawing(GameObject border, ref int leafNumber, ref GameObject emptyLeafs)
        {
            GameObject currentLeaf = new GameObject("Leaf" + leafNumber.ToString());
            currentLeaf.transform.SetParent(emptyLeafs.transform);

            int xPosition = x0;
            int yPosition = -2;
            int zPosition = z0;

            while(xPosition < x0 + length)
            {
                GameObject upperBorder = Instantiate(border);
                upperBorder.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                upperBorder.transform.SetParent(currentLeaf.transform);
                GameObject lowerBorder = Instantiate(border);
                lowerBorder.transform.position = (new Vector3(xPosition, yPosition, zPosition + width - 1));
                lowerBorder.transform.SetParent(currentLeaf.transform);
                xPosition++;
            }

            xPosition = x0;
            zPosition = z0 + 1;

            while(zPosition < z0 + width - 1)
            {
                GameObject leftBorder = Instantiate(border);
                leftBorder.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                leftBorder.transform.SetParent(currentLeaf.transform);
                GameObject rightBorder = Instantiate(border);
                rightBorder.transform.position = (new Vector3(xPosition + length - 1, yPosition, zPosition));
                rightBorder.transform.SetParent(currentLeaf.transform);
                zPosition++;
            }
        }
    }

    class Room 
    {
        public int x0;
        public int z0;
        
        public int width;
        public int length;
        public const int height = 5;

        GameObject emptyRoom;

        public int roomNumber;

        public Hall rightHall;
        public Hall upperHall;
        public Hall leftHall;
        public Hall lowerHall;

        /// <summary>
        /// constructor without parameters
        /// </summary>
        public Room()
        {
            x0 = 0;
            z0 = 0;

            width = 0;
            length = 0; 

            roomNumber = 0;

            emptyRoom = new GameObject("Room" + roomNumber.ToString());

            rightHall = null;
            upperHall = null;
            leftHall = null;
            lowerHall = null;
        }

        /// <summary>
        /// constructor with parameters without halls
        /// </summary>
        /// <param name="_x">x coordinate of lower left corner</param>
        /// <param name="_z">z coordinate of lower left corner</param>
        /// <param name="l">length (size on x coordinate)</param>
        /// <param name="w">width (size on z coordinate)</param>
        /// <param name="num">number of room</param>
        /// <param name="emptyRooms">main empty GameObject for hierarchy for storing all generated rooms</param>
        public Room(int _x, int _z, int l, int w, int num, ref GameObject emptyRooms) 
        {
            x0 = _x;
            z0 = _z;

            length = l;
            width = w;

            roomNumber = num;
            num++;

            emptyRoom = new GameObject("Room" + roomNumber.ToString());
            emptyRoom.transform.SetParent(emptyRooms.transform);

            rightHall = null;
            upperHall = null;
            leftHall = null;
            lowerHall = null;
        }

        /// <summary>
        /// constructor with parameters with one hall
        /// </summary>
        /// <param name="_x">x coordinate of lower left corner</param>
        /// <param name="_z">z coordinate of lower left corner</param>
        /// <param name="l">length (size on x coordinate)</param>
        /// <param name="w">width (size on z coordinate)</param>
        /// <param name="num">number of room</param>
        /// <param name="emptyRooms">main empty GameObject for hierarchy for storing all generated rooms</param>
        /// <param name="h">right or upper hall</param>
        public Room(int _x, int _z, int l, int w, int num, ref GameObject emptyRooms, Hall h)
        {
            x0 = _x;
            z0 = _z;

            length = l;
            width = w;

            roomNumber = num;
            num++;

            emptyRoom = new GameObject("Room" + roomNumber.ToString());
            emptyRoom.transform.SetParent(emptyRooms.transform);

            if (h.isHorizontal)
            {
                rightHall = h;
                upperHall = null;
            }
            else
            {
                upperHall = h;
                rightHall = null;
            }

            leftHall = null;
            lowerHall = null;
        }

        /// <summary>
        /// constructor with parameters with both halls
        /// </summary>
        /// <param name="_x">x coordinate of lower left corner</param>
        /// <param name="_z">z coordinate of lower left corner</param>
        /// <param name="l">length (size on x coordinate)</param>
        /// <param name="w">width (size on z coordinate)</param>
        /// <param name="num">number of room</param>
        /// <param name="emptyRooms">main empty GameObject for hierarchy for storing all generated rooms</param>
        /// <param name="right">horizontal hall</param>
        /// <param name="upper">vertical hall</param>
        public Room(int _x, int _z, int l, int w, int num, ref GameObject emptyRooms, Hall right, Hall upper)
        {
            x0 = _x;
            z0 = _z;

            length = l;
            width = w;

            roomNumber = num;
            num++;

            emptyRoom = new GameObject("Room" + roomNumber.ToString());
            emptyRoom.transform.SetParent(emptyRooms.transform);

            rightHall = right;
            upperHall = upper;
            leftHall = null;
            lowerHall = null;
        }

        public Vector3 GetRoomCenter()
        {
            int xCenter = x0 + length / 2;
            int zCenter = z0 + width / 2;
            return new Vector3(xCenter, 0f, zCenter);
        }

        public Vector3 GetRoomCenterWall()
        {
            int xCenter = x0 + length / 2;
            return new Vector3(xCenter, 0f, z0 + width - 1);
        }

        /// <summary>
        /// function for adding hall, that will be drawn
        /// </summary>
        /// <param name="hall">right or upper hall, that will be drawn</param>
        public void AddRealHall(Hall hall)
        {
            if (hall.isHorizontal)
                rightHall = hall;
            else
                upperHall = hall;
        }

        /// <summary>
        /// function for adding hall, which only be taken into account when drawing the passage
        /// </summary>
        /// <param name="hall">left or lower hall, that won't be drawn</param>
        public void AddImaginaryHall(Hall hall)
        {
            if (hall.isHorizontal)
                leftHall = hall;
            else
                lowerHall = hall;
        }

        /// <summary>
        /// function for drawing room
        /// </summary>
        /// <param name="floor">floor prefab</param>
        /// <param name="wall">wall prefab</param>
        public void RoomDrawing(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall)
        {
            FloorDrawing(floor);
            WallDrawing(lowerWall, wall, upperWall);

            if(rightHall != null)
                rightHall.HallDrawing(floor, lowerWall, wall, upperWall);

            if(upperHall != null)
                upperHall.HallDrawing(floor, lowerWall, wall, upperWall);
        }

        /// <summary>
        /// function for drawing room floor
        /// </summary>
        /// <param name="floor">floor prefab</param>
        private void FloorDrawing(GameObject floor)
        {
            int xPosition = x0;
            int yPosition = -1;
            int zPosition = z0;

            GameObject emptyFloor = new GameObject("Floor");
            emptyFloor.transform.SetParent(emptyRoom.transform);

            while(zPosition < z0 + width)
            {
                while(xPosition < x0 + length)
                {
                    //ground
                    GameObject lowerFloor = Instantiate(floor);
                    lowerFloor.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                    lowerFloor.transform.SetParent(emptyFloor.transform);
                    //ceiling
                    GameObject upperFloor = Instantiate(floor);
                    upperFloor.transform.position = (new Vector3(xPosition, yPosition + height + 1, zPosition));
                    upperFloor.transform.SetParent(emptyFloor.transform);

                    xPosition++;
                }

                xPosition = x0;
                zPosition++;
            }
        }

        /// <summary>
        /// function for drawing room walls
        /// </summary>
        /// <param name="wall">wall prefab</param>
        private void WallDrawing(GameObject lowerWall, GameObject wall, GameObject upperWall)
        {
            int xPosition = x0;
            int yPosition = 0;
            int zPosition = z0;

            GameObject emptyWall = new GameObject("Walls");
            emptyWall.transform.SetParent(emptyRoom.transform);

            while(xPosition < x0 + length)
            {
                while(yPosition < height)
                {
                    //��������� ������ �����
                    if(lowerHall == null || lowerHall != null && 
                       !(xPosition > lowerHall.x0 && xPosition < (lowerHall.x0 + lowerHall.width - 1)))
                    {
                        GameObject currentWall;
                        if (yPosition == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (yPosition == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                        currentWall.transform.SetParent(emptyWall.transform);
                    }

                    //��������� ������� �����
                    if (upperHall == null || upperHall != null && 
                        !(xPosition > upperHall.x0 && xPosition < (upperHall.x0 + upperHall.width - 1)))
                    {
                        GameObject currentWall;
                        if (yPosition == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (yPosition == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition, yPosition, zPosition + width - 1));
                        currentWall.transform.SetParent(emptyWall.transform);
                    }

                    yPosition++;
                }
                xPosition++;
                yPosition = 0;
            }

            xPosition = x0;
            yPosition = 0;
            zPosition = z0 + 1;

            while(zPosition < z0 + width)
            {
                while(yPosition < height)
                {
                    //��������� ����� �����
                    if(leftHall == null || leftHall != null &&
                        !(zPosition > leftHall.z0 && zPosition < (leftHall.z0 + leftHall.width - 1)))
                    {
                        GameObject currentWall;
                        if (yPosition == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (yPosition == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                        currentWall.transform.SetParent(emptyWall.transform);
                    }

                    //��������� ������ �����
                    if (rightHall == null || rightHall != null && 
                        !(zPosition > rightHall.z0 && zPosition < (rightHall.z0 + rightHall.width - 1)))
                    {
                        GameObject currentWall;
                        if (yPosition == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (yPosition == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition + length - 1, yPosition, zPosition));
                        currentWall.transform.SetParent(emptyWall.transform);
                    }

                    yPosition++;
                }
                
                zPosition++;
                yPosition = 0;
            }
        }
    }

    class Hall
    {
        public int x0;
        public int z0;

        public int length;
        public int width = 4;
        public const int height = 5;

        public bool isHorizontal;

        GameObject emptyHall;

        public int leftRoomNumber;
        public int rightRoomNumber;

        /// <summary>
        /// constructor without parameters
        /// </summary>
        public Hall()
        {
            x0 = 0;
            z0 = 0;

            length = 0;

            isHorizontal = true;

            leftRoomNumber = 0;
            rightRoomNumber = 0;

            emptyHall = new GameObject("Hall" + leftRoomNumber.ToString() + rightRoomNumber.ToString());
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        /// <param name="_x">x coordinate of lower left corner</param>
        /// <param name="_z">z coordinate of lower left corner</param>
        /// <param name="l">size of hall</param>
        /// <param name="iH">parameter, showing is hall horizontal (true) or vertical (false)</param>
        /// <param name="leftNum">number of left room, connected with this hall</param>
        /// <param name="rightNum">number or right room, connected with this hall</param>
        /// <param name="emptyHalls">main empty GameObject for hierarchy for storing all generated rooms</param>
        public Hall(int _x, int _z, int l, bool iH, int leftNum, int rightNum, ref GameObject emptyHalls)
        {
            x0 = _x;
            z0 = _z;

            length = l;

            isHorizontal = iH;

            leftRoomNumber = leftNum;
            rightRoomNumber = rightNum;

            emptyHall = new GameObject("Hall" + leftRoomNumber.ToString() + rightRoomNumber.ToString());
            emptyHall.transform.SetParent(emptyHalls.transform);
        }

        /// <summary>
        /// function for drawing hall
        /// </summary>
        /// <param name="floor">floor prefab</param>
        /// <param name="wall">wall prefab</param>
        public void HallDrawing(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall)
        {
            GroundDrawing(floor);
            WallDrawing(lowerWall, wall, upperWall);
        }

        /// <summary>
        /// function for drawing hall ground and ceiling
        /// </summary>
        /// <param name="floor">floor prefab</param>
        private void GroundDrawing(GameObject floor)
        {
            int xPosition = x0;
            int yPosition = -1;
            int zPosition = z0;

            if(isHorizontal)
            {
                while(xPosition < x0 + length)
                {
                    for(int i = z0; i < z0 + width; i++)
                    {
                        //ground
                        GameObject currentGround = Instantiate(floor);
                        currentGround.transform.position = new Vector3(xPosition, yPosition, i);
                        currentGround.transform.SetParent(emptyHall.transform);
                        //ceiling
                        GameObject currentCeiling = Instantiate(floor);
                        currentCeiling.transform.position = new Vector3(xPosition, yPosition + height + 1, i);
                        currentCeiling.transform.SetParent(emptyHall.transform);
                    }
                    xPosition++;
                }
            }
            else
            {
                while(zPosition < z0 + length)
                {
                    for(int i = x0; i < x0 + width; i++)
                    {
                        //ground
                        GameObject currentGround = Instantiate(floor);
                        currentGround.transform.position = new Vector3(i, yPosition, zPosition);
                        currentGround.transform.SetParent(emptyHall.transform);
                        //ceiling
                        GameObject currentCeiling = Instantiate(floor);
                        currentCeiling.transform.position = new Vector3(i, yPosition + height + 1, zPosition);
                        currentCeiling.transform.SetParent(emptyHall.transform);
                    }
                    zPosition++;
                }
            }
        }

        /// <summary>
        /// function for drawing hall walls
        /// </summary>
        /// <param name="wall">wall prefab</param>
        private void WallDrawing(GameObject lowerWall, GameObject wall, GameObject upperWall)
        {
            int xPosition = x0;
            int yPosition = 0;
            int zPosition = z0;

            if(isHorizontal)
            {
                while(xPosition < x0 + length)
                {
                    for(int i = yPosition; i < yPosition + height; i++)
                    {
                        GameObject currentWall;
                        if (i == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (i == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition, i, zPosition));
                        currentWall.transform.SetParent(emptyHall.transform);
                        if (i == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (i == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition, i, zPosition + width - 1));
                        currentWall.transform.SetParent(emptyHall.transform);
                    }
                    xPosition++;
                }
            }
            else
            {
                while(zPosition < z0 + length)
                {
                    for(int i = yPosition; i < yPosition + height; i++)
                    {
                        GameObject currentWall;
                        if (i == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (i == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition, i, zPosition));
                        currentWall.transform.SetParent(emptyHall.transform);
                        if (i == 0)
                        {
                            currentWall = Instantiate(lowerWall);
                        }
                        else if (i == height - 1)
                        {
                            currentWall = Instantiate(upperWall);
                        }
                        else
                        {
                            currentWall = Instantiate(wall);
                        }
                        currentWall.transform.position = (new Vector3(xPosition + width - 1, i, zPosition));
                        currentWall.transform.SetParent(emptyHall.transform);
                    }
                    zPosition++;
                }
            }
        }
    }
}