using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//x - length
//y - height
//z - width

public class GeterateTunnelMap : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject upperWall;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject lowerWall;

    private void Start()
    {
        //CircleRoom circleRoom1 = new CircleRoom(-1, -1, 5);
        //circleRoom.DrawCircleRoom(ground, wall);

        //CircleRoom circleRoom2 = new CircleRoom(3, 6, 9);
        //circleRoom2.DrawCircleRoom(ground, wall);

        //CircleRoom circleRoom1 = new CircleRoom(Random.Range(-1, 5), Random.Range(-1, 5), Random.Range(4, 8));
        //circleRoom.DrawCircleRoom(ground, wall);

        //CircleRoom circleRoom2 = new CircleRoom(Random.Range(3, 8), Random.Range(3, 8), Random.Range(6, 10));
        //circleRoom2.DrawCircleRoom(ground, wall);

        //RectangleRoom room1 = new RectangleRoom(0, 0, 15, 20);
        //room.DrawRectangleRoom(ground, wall);

        //RectangleRoom room2 = new RectangleRoom(10, 10, 15, 15);

        //CompositeRoom compositeRoom = new CompositeRoom(room1, room2);
        //compositeRoom.DrawCompositeRoom(ground, wall);

        //CompositeRoom compositecircleRoom = new CompositeRoom(circleRoom1, circleRoom2);
        //compositecircleRoom.DrawCompositeRoom(ground, wall);

        int numberOfRoom = 1;
        GameObject emptyRooms = new GameObject("Rooms");

        TunnelMap map = new TunnelMap(0, 0, 100, 100);
        map.GenerateFirstRoom(ref numberOfRoom, ref emptyRooms);
        map.GenerateRoom(ref numberOfRoom, ref emptyRooms);
        //for (int i = 0; i < 2; i++)
        //{
        //    map.GenerateRoom();
        //}
        map.DrawMap(ground, lowerWall, wall, upperWall);
    }

    class TunnelMap
    {
        public int x0;
        public int z0;

        public int width;
        public int length;

        public List<CompositeRoom> rooms;

        private const int minRoomSize = 6;
        private const int maxRoomSize = 12;

        public TunnelMap()
        {
            x0 = 0;
            z0 = 0;

            width = 0;
            length = 0;

            rooms = null;
        }

        public TunnelMap(int _x0, int _z0, int _width, int _length)
        {
            x0 = _x0;
            z0 = _z0;

            width = _width;
            length = _length;

            rooms = new List<CompositeRoom>();
        }

        //public void GenerateRectangleRoom()
        //{
        //    bool flag = true;
        //    RectangleRoom room = null;
        //    while (flag)
        //    {
        //        int x0 = Random.Range(_x0, _x0 + _width);
        //        int z0 = Random.Range(_z0, _z0 + _length);
        //        int width = Random.Range(_minRoomSize, _maxRoomSize);
        //        int length = Random.Range(_minRoomSize, _maxRoomSize);
        //        room = new RectangleRoom(x0, z0, width, length);
        //        bool isIntersect = false;
        //        for(int i = 0; i < _rooms.Count && !isIntersect; i++)
        //        {
        //            if (room.IsRoomsIntersect(_rooms[i]))
        //            {
        //                isIntersect = true;
        //            }
        //        }
        //        if(!isIntersect)
        //        {
        //            flag = false;
        //        }
        //        else
        //        {
        //            room.DestroyRoom();
        //        }
        //    }

        //    if (room != null)
        //    {
        //        _rooms.Add(room);
        //    }
        //}

        public (CircleRoom, CircleRoom) GetCompositeRooms(ref int numberOfRoom)
        {
            CircleRoom room1 = null;
            CircleRoom room2 = null;
            int roomX = Random.Range(x0, x0 + width);
            int roomZ = Random.Range(z0, z0 + length);
            int radius = Random.Range(minRoomSize, maxRoomSize);
            room1 = new CircleRoom(roomX, roomZ, radius, numberOfRoom);
            numberOfRoom++;

            roomX = Random.Range(x0, x0 + width);
            roomZ = Random.Range(z0, z0 + length);
            radius = Random.Range(minRoomSize, maxRoomSize);
            room2 = new CircleRoom(roomX, roomZ, radius, numberOfRoom);

            bool isCompositeRoom = room1.IsRoomsIntersect(room2);
            while (!isCompositeRoom)
            {
                room2.DestroyRoom();
                roomX = Random.Range(x0, x0 + width);
                roomZ = Random.Range(z0, z0 + length);
                radius = Random.Range(minRoomSize, maxRoomSize);
                room2 = new CircleRoom(roomX, roomZ, radius, numberOfRoom);
                isCompositeRoom = room1.IsRoomsIntersect(room2);
            }
            numberOfRoom++;
            return (room1, room2);
        }

        public void GenerateFirstRoom(ref int numberOfRoom, ref GameObject emptyRooms)
        {
            CircleRoom room1 = null;
            CircleRoom room2 = null;

            int radius = Random.Range(minRoomSize, maxRoomSize);
            int centerX = (x0 + width) / 2;
            int centerZ = (z0 + length) / 2;
            int roomX = Random.Range(centerX - radius, centerX + radius);
            int roomZ = Random.Range(centerZ - radius, centerZ + radius);
            room1 = new CircleRoom(roomX, roomZ, radius, numberOfRoom);
            numberOfRoom++;

            roomX = Random.Range(x0, x0 + width);
            roomZ = Random.Range(z0, z0 + length);
            radius = Random.Range(minRoomSize, maxRoomSize);
            room2 = new CircleRoom(roomX, roomZ, radius, numberOfRoom);

            bool isCompositeRoom = room1.IsRoomsIntersect(room2);
            while (!isCompositeRoom)
            {
                room2.DestroyRoom();
                roomX = Random.Range(x0, x0 + width);
                roomZ = Random.Range(z0, z0 + length);
                radius = Random.Range(minRoomSize, maxRoomSize);
                room2 = new CircleRoom(roomX, roomZ, radius, numberOfRoom);
                isCompositeRoom = room1.IsRoomsIntersect(room2);
            }
            numberOfRoom++;

            //(room1, room2) = GetCompositeRooms(ref numberOfRoom);

            CompositeRoom compositeRoom = new CompositeRoom(room1, room2, numberOfRoom - 2, numberOfRoom - 1, ref emptyRooms);
            rooms.Add(compositeRoom);
        }

        public void GenerateRoom(ref int numberOfRoom, ref GameObject emptyRooms)
        {
            int indexOfRoom = Random.Range(0, rooms.Count);
            bool flag = true;
            while(flag)
            {
                int side = Random.Range(0, 3);
                int closeRoomX;
                int closeRoomZ;
                int factorX;
                int factorZ;
                if(side == 0)
                {
                    (closeRoomX, closeRoomZ) = rooms[indexOfRoom].GetMinZ();
                    factorX = 1;
                    factorZ = -1;
                }
                else if(side == 1)
                {
                    (closeRoomX, closeRoomZ) = rooms[indexOfRoom].GetMaxZ();
                    factorX = 1;
                    factorZ = 1;
                }
                else if(side == 2)
                {
                    (closeRoomX, closeRoomZ) = rooms[indexOfRoom].GetMinX();
                    factorX = -1;
                    factorZ = 1;
                }
                else if(side == 3)
                {
                    (closeRoomX, closeRoomZ) = rooms[indexOfRoom].GetMaxX();
                    factorX = 1;
                    factorZ = 1;
                }

            }
        }

        public void GenerateRandomRoom(ref int numberOfRoom, ref GameObject emptyRooms)
        {
            bool flag = true;
            CircleRoom room1 = null;
            CircleRoom room2 = null;
            CompositeRoom compositeRoom = null;
            int countOfAttempts = 0;
            while (flag && countOfAttempts < 5)
            {
                (room1, room2) = GetCompositeRooms(ref numberOfRoom);

                compositeRoom = new CompositeRoom(room1 , room2, numberOfRoom - 2, numberOfRoom - 1, ref emptyRooms);
                bool isIntersect = false;
                for (int i = 0; i < rooms.Count && !isIntersect; i++)
                {
                    if (compositeRoom.isRoomIntersect(rooms[i]))
                    {
                        isIntersect = true;
                    }
                }
                if (!isIntersect)
                {
                    flag = false;
                }
                else
                {
                    compositeRoom.DestroyRooms();
                    numberOfRoom -= 2;
                }
                countOfAttempts++;
            }

            if (compositeRoom != null)
            {
                rooms.Add(compositeRoom);
            }
        }

        public void DrawMap(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall)
        {
            foreach(CompositeRoom room in rooms)
            {
                room.DrawCompositeRoom(floor, lowerWall, wall, upperWall);
            }
        }
    }

    class Room
    {
        public int x0;
        public int z0;

        public const int height = 5;

        public GameObject emptyRoom;

        public int index;

        public Room()
        {
            x0 = 0;
            z0 = 0;

            //emptyRoom = new GameObject("Room");
        }

        public Room(int x0, int z0)
        {
            this.x0 = x0;
            this.z0 = z0;

            //emptyRoom = new GameObject("Room");
        }

        public virtual bool IsDotInsideRoom(int x, int z)
        {
            return true;
        }

        public virtual void DrawRoom(GameObject floor, GameObject wall) { }

        public virtual void DrawPartOfRoom(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall, Room room) { }

        public virtual bool IsRoomsIntersect(Room room)
        { 
            return true;
        }

        public virtual int GetMinX()
        {
            return 0;
        }

        public virtual int GetMaxX()
        {
            return 0;
        }

        public virtual int GetMinZ()
        {
            return 0;
        }

        public virtual int GetMaxZ()
        {
            return 0;
        }

        public virtual void DestroyRoom() { }
    }

    class RectangleRoom : Room
    {
        public int width;
        public int length;

        public RectangleRoom()
        {
            x0 = 0;
            z0 = 0;

            width = 0;
            length = 0;

            //emptyRoom = new GameObject("Room");
        }

        public RectangleRoom(int x0, int z0, int width, int length)
        {
            this.x0 = x0;
            this.z0 = z0;

            this.width = width;
            this.length = length;

            emptyRoom = new GameObject("Room");
        }

        public void GenerateRectangleRoomParameters()
        {
            x0 = Random.Range(0, 30);
            z0 = Random.Range(0, 30);

            width = Random.Range(0, 30);
            length = Random.Range(0, 30);
        }

        public override bool IsDotInsideRoom(int x, int z)
        {
            if(x >= x0 && x < x0 + length - 1 && z >= z0 && z < z0 + width - 1)
                return true;
            else 
                return false;
        }

        public override void DrawRoom(GameObject floor, GameObject wall)
        {
            FloorDrawing(floor);
            WallDrawing(wall);
        }

        public override void DrawPartOfRoom(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall, Room room)
        {
            FloorDrawing(floor);
            CompositeWallDrawing(wall, room);
        }

        public void FloorDrawing(GameObject floor) 
        {
            int xPosition = x0;
            int yPosition = -1;
            int zPosition = z0;

            GameObject emptyFloor = new GameObject("Floor");
            emptyFloor.transform.SetParent(emptyRoom.transform);

            while (zPosition < z0 + width)
            {
                while (xPosition < x0 + length)
                {
                    GameObject lowerFloor = Instantiate(floor);
                    lowerFloor.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                    lowerFloor.transform.SetParent(emptyFloor.transform);
                    //отрисовка потолков
                    //GameObject upperFloor = Instantiate(floor);
                    //upperFloor.transform.position = (new Vector3(xPosition, yPosition + height + 1, zPosition));
                    //upperFloor.transform.SetParent(emptyFloor.transform);

                    xPosition++;
                }

                xPosition = x0;
                zPosition++;
            }
        }

        public void WallDrawing(GameObject wall)
        {
            int xPosition = x0;
            int yPosition = 0;
            int zPosition = z0;

            GameObject emptyWall = new GameObject("Walls");
            emptyWall.transform.SetParent(emptyRoom.transform);

            while (xPosition < x0 + length)
            {
                while (yPosition < height)
                {
                    //отрисовка нижней стены
                    GameObject lowerWall = Instantiate(wall);
                    lowerWall.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                    lowerWall.transform.SetParent(emptyWall.transform);

                    //отрисовка верхней стены
                    GameObject upperWall = Instantiate(wall);
                    upperWall.transform.position = (new Vector3(xPosition, yPosition, zPosition + width - 1));
                    upperWall.transform.SetParent(emptyWall.transform);

                    yPosition++;
                }
                xPosition++;
                yPosition = 0;
            }

            xPosition = x0;
            yPosition = 0;
            zPosition = z0 + 1;

            while (zPosition < z0 + width)
            {
                while (yPosition < height)
                {
                    //отрисовка левой стены
                    GameObject leftWall = Instantiate(wall);
                    leftWall.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                    leftWall.transform.SetParent(emptyWall.transform);

                    //отрисовка правой стены
                    GameObject rightWall = Instantiate(wall);
                    rightWall.transform.position = (new Vector3(xPosition + length - 1, yPosition, zPosition));
                    rightWall.transform.SetParent(emptyWall.transform);

                    yPosition++;
                }

                zPosition++;
                yPosition = 0;
            }
        }

        public void CompositeWallDrawing(GameObject wall, Room room)
        {
            int xPosition = x0;
            int yPosition = 0;
            int zPosition = z0;

            GameObject emptyWall = new GameObject("Walls");
            emptyWall.transform.SetParent(emptyRoom.transform);

            while (xPosition < x0 + length)
            {
                while (yPosition < height)
                {
                    //отрисовка нижней стены
                    if (!room.IsDotInsideRoom(xPosition, zPosition))
                    {
                        GameObject lowerWall = Instantiate(wall);
                        lowerWall.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                        lowerWall.transform.SetParent(emptyWall.transform);
                    }

                    //отрисовка верхней стены
                    if (!room.IsDotInsideRoom(xPosition, zPosition + width - 1))
                    {
                        GameObject upperWall = Instantiate(wall);
                        upperWall.transform.position = (new Vector3(xPosition, yPosition, zPosition + width - 1));
                        upperWall.transform.SetParent(emptyWall.transform);
                    }

                    yPosition++;
                }
                xPosition++;
                yPosition = 0;
            }

            xPosition = x0;
            yPosition = 0;
            zPosition = z0 + 1;

            while (zPosition < z0 + width)
            {
                while (yPosition < height)
                {
                    //отрисовка левой стены
                    if (!room.IsDotInsideRoom(xPosition, zPosition))
                    {
                        GameObject leftWall = Instantiate(wall);
                        leftWall.transform.position = (new Vector3(xPosition, yPosition, zPosition));
                        leftWall.transform.SetParent(emptyWall.transform);
                    }

                    //отрисовка правой стены
                    if (!room.IsDotInsideRoom(xPosition + length - 1, zPosition))
                    {
                        GameObject rightWall = Instantiate(wall);
                        rightWall.transform.position = (new Vector3(xPosition + length - 1, yPosition, zPosition));
                        rightWall.transform.SetParent(emptyWall.transform);
                    }

                    yPosition++;
                }

                zPosition++;
                yPosition = 0;
            }
        }

        public override bool IsRoomsIntersect(Room room)
        {
            for(int i = x0; i <= x0 + width; i++)
            {
                for(int j = z0; j <= z0 + length; j++)
                {
                    if (room.IsDotInsideRoom(i, j))
                        return true;
                }
            }
            return false;
        }

        public override void DestroyRoom()
        {
            Destroy(emptyRoom);
        }
    }

    class CircleRoom : Room
    {
        public int radius;

        public CircleRoom()
        {
            x0 = 0;
            z0 = 0;

            radius = 0;

            //emptyRoom = new GameObject("Room");
        }

        public CircleRoom(int x0, int z0, int radius, int numberOfRoom)
        {
            this.x0 = x0;
            this.z0 = z0;

            this.radius = radius;

            emptyRoom = new GameObject("Room" + numberOfRoom);
        }

        public override bool IsDotInsideRoom(int x, int z)
        {
            //int radiusSqrt = (int)Mathf.Sqrt(radius);
            int radiusSqrt = radius / 2;
            int width;
            int difference;
            if (z == z0 || z == (z0 + radius + 2 * radiusSqrt + 1))
            {
                width = radius;
                difference = 0;
            }
            else if (z > z0 && z <= (z0 + radiusSqrt))
            {
                width = radius + 2 * (z - z0);
                difference = z - z0;
            }
            else if(z < (z0 + radius + 2 * radiusSqrt + 1) && z > (z0 + radiusSqrt + radius))
            {
                width = radius + 2 * (z0 + radius + 2 * radiusSqrt + 1 - z);
                difference = z0 + radius + 2 * radiusSqrt + 1 - z;
            }
            else
            {
                width = radius + 2 * radiusSqrt + 2;
                difference = radiusSqrt + 1;
            }

            if (!(x > (x0 - difference) && x < (x0 - difference + width - 1)))
            {
                return false;
            }

            if (x == (x0 - radiusSqrt - 1) || x == (x0 + radius + radiusSqrt))
            {
                width = radius;
                difference = radiusSqrt + 1;
            }
            else if (x > (x0 - radiusSqrt - 1) && x < x0)
            {
                width = radius + 2 * (x - x0 + radiusSqrt + 1);
                difference = x0 - x;
            }
            else if (x >= (x0 + radius) && x < (x0 + radius + radiusSqrt))
            {
                width = radius + 2 * (radiusSqrt - (x - (x0 + radius)));
                //difference = x - x0 - radius - 1;
                difference = x - (x0 + radius);
            }
            else
            {
                width = radius + 2 * radiusSqrt + 2;
                difference = 0;
            }

            if (z >= z0 + difference && z <= z0 + difference + width - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void DrawRoom(GameObject floor, GameObject wall)
        {
            FloorDrawing(floor);
            WallDrawing(wall);
        }

        public override void DrawPartOfRoom(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall, Room room)
        {
            FloorDrawing(floor);
            CompositeWallDrawing(lowerWall, wall, upperWall, room);
        }

        private void FloorDrawing(GameObject floor)
        {
            GameObject emptyFloor = new GameObject("Floor");
            emptyFloor.transform.SetParent(emptyRoom.transform);

            int y0 = -1;
            int currentX;
            int currentZ = z0;
            //int radiusSqrt = (int)Mathf.Sqrt(radius);
            int radiusSqrt = radius / 2;
            int counter = 1;
            int flag = 1;
            int width;
            while (currentZ < (z0 + radius + 2 * radiusSqrt + 2))
            {
                currentX = x0;
                if (currentZ == z0 || currentZ == (z0 + radius + 2 * radiusSqrt + 1))
                {
                    width = radius;
                }
                else if ((currentZ > z0 && currentZ <= (z0 + radiusSqrt)) || (currentZ < (z0 + radius + 2 * radiusSqrt + 1) && currentZ >= (z0 + radiusSqrt + radius)))
                {
                    width = radius + 2 * counter;
                    currentX -= Mathf.Abs(counter);
                    counter += flag;
                }
                else
                {
                    width = radius + 2 * radiusSqrt + 2;
                    flag = -1;
                    currentX = x0 - radiusSqrt - 1;
                }
                int x = currentX;
                while (currentX < x + width)
                {
                    GameObject currentFloor = Instantiate(floor);
                    currentFloor.transform.position = (new Vector3(currentX, y0, currentZ));
                    currentFloor.transform.SetParent(emptyFloor.transform);
                    currentX++;
                }
                currentZ++;
            }
        }

        public void WallDrawing(GameObject wall)
        {
            GameObject emptyWall = new GameObject("Walls");
            emptyWall.transform.SetParent(emptyRoom.transform);

            int currentX;
            int currentZ = z0;
            int currentY;
            int height = 4;
            //radiusSqrt = (int)Mathf.Sqrt(radius);
            int radiusSqrt = radius / 2;
            int counter = 1;
            int flag = 1;
            int width;

            while (currentZ < (z0 + radius + 2 * radiusSqrt + 2))
            {
                currentX = x0;
                currentY = 0; // или -1
                if (currentZ == z0 || currentZ == (z0 + radius + 2 * radiusSqrt + 1))
                {
                    width = radius;
                }
                else if ((currentZ > z0 && currentZ <= (z0 + radiusSqrt)) || (currentZ < (z0 + radius + 2 * radiusSqrt + 1) && currentZ >= (z0 + radiusSqrt + radius)))
                {
                    width = radius + 2 * counter;
                    currentX -= Mathf.Abs(counter);
                    counter += flag;
                }
                else
                {
                    width = radius + 2 * radiusSqrt + 2;
                    flag = -1;
                    currentX = x0 - radiusSqrt - 1;
                }
                while (currentY < height)
                {
                    if (width == radius)
                    {
                        int x = currentX;
                        while (currentX < x + width)
                        {
                            GameObject currentWall = Instantiate(wall);
                            currentWall.transform.position = (new Vector3(currentX, currentY, currentZ));
                            currentWall.transform.SetParent(emptyWall.transform);
                            currentX++;
                        }
                        currentX = x;
                    }
                    else
                    {
                        GameObject leftWall = Instantiate(wall);
                        leftWall.transform.position = (new Vector3(currentX, currentY, currentZ));
                        leftWall.transform.SetParent(emptyWall.transform);
                        GameObject rightWall = Instantiate(wall);
                        rightWall.transform.position = (new Vector3(currentX + width - 1, currentY, currentZ));
                        rightWall.transform.SetParent(emptyWall.transform);
                    }
                    currentY++;
                }
                currentZ++;
            }
        }

        public void CompositeWallDrawing(GameObject lowerWall, GameObject wall, GameObject upperWall, Room room)
        {
            GameObject emptyWall = new GameObject("Walls");
            emptyWall.transform.SetParent(emptyRoom.transform);

            int currentX;
            int currentZ = z0;
            int currentY;
            //radiusSqrt = (int)Mathf.Sqrt(radius);
            int radiusSqrt = radius / 2;
            int counter = 1;
            int flag = 1;
            int width;

            while (currentZ < (z0 + radius + 2 * radiusSqrt + 2))
            {
                currentX = x0;
                currentY = 0; // или -1
                if (currentZ == z0 || currentZ == (z0 + radius + 2 * radiusSqrt + 1))
                {
                    width = radius;
                }
                else if ((currentZ > z0 && currentZ <= (z0 + radiusSqrt)) || (currentZ < (z0 + radius + 2 * radiusSqrt + 1) && currentZ >= (z0 + radiusSqrt + radius)))
                {
                    width = radius + 2 * counter;
                    currentX -= Mathf.Abs(counter);
                    counter += flag;
                }
                else
                {
                    width = radius + 2 * radiusSqrt + 2;
                    flag = -1;
                    currentX = x0 - radiusSqrt - 1;
                }
                while (currentY < height)
                {
                    if (width == radius)
                    {
                        int x = currentX;
                        while (currentX < x + width)
                        {
                            if (!room.IsDotInsideRoom(currentX, currentZ))
                            {
                                GameObject currentWall;
                                if (currentY == 0)
                                {
                                    currentWall = Instantiate(lowerWall);
                                }
                                else if (currentY == height - 1)
                                {
                                    currentWall = Instantiate(upperWall);
                                }
                                else
                                {
                                    currentWall = Instantiate(wall);
                                }
                                currentWall.transform.position = (new Vector3(currentX, currentY, currentZ));
                                currentWall.transform.SetParent(emptyWall.transform);
                            }
                            currentX++;
                        }
                        currentX = x;
                    }
                    else
                    {
                        if (!room.IsDotInsideRoom(currentX, currentZ))
                        {
                            GameObject leftWall;
                            if (currentY == 0)
                            {
                                leftWall = Instantiate(lowerWall);
                            }
                            else if (currentY == height - 1)
                            {
                                leftWall = Instantiate(upperWall);
                            }
                            else
                            {
                                leftWall = Instantiate(wall);
                            }
                            leftWall.transform.position = (new Vector3(currentX, currentY, currentZ));
                            leftWall.transform.SetParent(emptyWall.transform);
                        }
                        if (!room.IsDotInsideRoom(currentX + width - 1, currentZ))
                        {
                            GameObject rightWall;
                            if (currentY == 0)
                            {
                                rightWall = Instantiate(lowerWall);
                            }
                            else if (currentY == height - 1)
                            {
                                rightWall = Instantiate(upperWall);
                            }
                            else
                            {
                                rightWall = Instantiate(wall);
                            }
                            rightWall.transform.position = (new Vector3(currentX + width - 1, currentY, currentZ));
                            rightWall.transform.SetParent(emptyWall.transform);
                        }
                    }
                    currentY++;
                }
                currentZ++;
            }
        }

        public override bool IsRoomsIntersect(Room room)
        {
            int currentX;
            int currentZ = z0;
            //int radiusSqrt = (int)Mathf.Sqrt(radius);
            int radiusSqrt = radius / 2;
            int counter = 1;
            int flag = 1;
            int width;
            bool isIntersect = false;
            while (currentZ < (z0 + radius + 2 * radiusSqrt + 2) && !isIntersect)
            {
                currentX = x0;
                if (currentZ == z0 || currentZ == (z0 + radius + 2 * radiusSqrt + 1))
                {
                    width = radius;
                }
                else if ((currentZ > z0 && currentZ <= (z0 + radiusSqrt)) || (currentZ < (z0 + radius + 2 * radiusSqrt + 1) && currentZ >= (z0 + radiusSqrt + radius)))
                {
                    width = radius + 2 * counter;
                    currentX -= Mathf.Abs(counter);
                    counter += flag;
                }
                else
                {
                    width = radius + 2 * radiusSqrt + 2;
                    flag = -1;
                    currentX = x0 - radiusSqrt - 1;
                }
                int x = currentX;
                while (currentX < x + width && !isIntersect)
                {
                    isIntersect = room.IsDotInsideRoom(currentX, currentZ);
                    currentX++;
                }
                currentZ++;
            }
            return isIntersect;
        }

        public override int GetMinX()
        {
            return x0 - radius / 2 - 1;
        }

        public override int GetMaxX()
        {
            return x0 + radius + radius / 2;
        }

        public override int GetMinZ()
        {
            return z0;
        }

        public override int GetMaxZ()
        {
            return z0 + radius + 2 * (radius / 2) + 1;
        }

        public override void DestroyRoom()
        {
            Destroy(emptyRoom);
        }
    }

    class CompositeRoom
    {
        public Room room1;
        public Room room2;

        private GameObject emptyRoom;

        public CompositeRoom()
        {
            room1 = new Room();
            room2 = new Room();
        }

        public CompositeRoom(Room room1, Room room2, int index1, int index2, ref GameObject emptyRooms)
        {
            this.room1 = room1;
            this.room2 = room2;

            emptyRoom = new GameObject("Room" + index1 + index2);
            emptyRoom.transform.SetParent(emptyRooms.transform);
        }

        public bool isRoomIntersect(CompositeRoom room)
        {
            bool flag = false;
            flag = room1.IsRoomsIntersect(room.room1);
            if(!flag)
            {
                flag = room1.IsRoomsIntersect(room.room2);
            }
            if(!flag)
            {
                flag = room2.IsRoomsIntersect(room.room1);
            }
            if(!flag)
            {
                flag = room2.IsRoomsIntersect(room.room2);
            }
            return flag;
        }

        public void DrawCompositeRoom(GameObject floor, GameObject lowerWall, GameObject wall, GameObject upperWall)
        {
            room1.emptyRoom.transform.SetParent(emptyRoom.transform);
            room2.emptyRoom.transform.SetParent(emptyRoom.transform);
            room1.DrawPartOfRoom(floor, lowerWall, wall, upperWall, room2);
            room2.DrawPartOfRoom(floor, lowerWall, wall, upperWall, room1);
        }

        public (int, int) GetMinX()
        {
            int min1 = room1.GetMinX();
            int min2 = room2.GetMinX();
            if(min1 < min2)
            {
                int centerZ = (room1.GetMaxZ() - room1.GetMinZ()) / 2 + room1.GetMinZ();
                return (centerZ, min1);
            }
            else
            {
                int centerZ = (room2.GetMaxZ() - room2.GetMinZ()) / 2 + room2.GetMinZ();
                return (centerZ, min2);
            }
        }

        public (int, int) GetMaxX()
        {
            int max1 = room1.GetMaxX();
            int max2 = room2.GetMaxX();
            if(max1 > max2)
            {
                int centerZ = (room1.GetMaxZ() - room1.GetMinZ()) / 2 + room1.GetMinZ();
                return (max1, centerZ);
            }
            else
            {
                int centerZ = (room2.GetMaxZ() - room2.GetMinZ()) / 2 + room2.GetMinZ();
                return (max2, centerZ);
            }
        }

        public (int, int) GetMinZ()
        {
            int min1 = room1.GetMinZ();
            int min2 = room2.GetMinZ();
            if (min1 < min2)
            {
                int centerX = (room1.GetMaxX() - room1.GetMinX()) / 2 + room1.GetMinX();
                return (centerX, min1);
            }
            else
            {
                int centerX = (room2.GetMaxX() - room2.GetMinX()) / 2 + room2.GetMinX();
                return (centerX, min2);
            }
        }

        public (int, int) GetMaxZ()
        {
            int max1 = room1.GetMaxZ();
            int max2 = room2.GetMaxZ();
            if (max1 > max2)
            {
                int centerX = (room1.GetMaxX() - room1.GetMinX()) / 2 + room1.GetMinX();
                return (centerX, max1);
            }
            else
            {
                int centerX = (room2.GetMaxX() - room2.GetMinX()) / 2 + room2.GetMinX();
                return (centerX, max2);
            }
        }

        public void DestroyRooms()
        {
            room1.DestroyRoom();
            room2.DestroyRoom();
            Destroy(emptyRoom);
        }
    }
}
