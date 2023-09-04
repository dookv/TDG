#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace ZeEngine
{
    public class SquareGrid
    {
        public bool showGrid;//show the grid
        public Basic2d gridImg;
        public Vector2 slotDims, gridDims, physicalStartPos, totalPhysicalDims, currentHoverSlot;//physicalStartPos = top left corner of the grid,
                                                                                                 //totalPhysicalDims(super simple caching) = total length and width of the grid(slotdims * slots or gridDims)
                                                                                                 //currentHoverSlot = caching
        

        public List<List<GridLocation>> slots = new List<List<GridLocation>>();//like a 2 dimentional array, arrays are more efficient, lists cost more resources, If you game is lagging becasue of your grid processing you have to change this to an array
        public List<GridItem> gridItems = new List<GridItem>();

        public SquareGrid(Vector2 SLOTDIMS, Vector2 STARTPOS, Vector2 TOTALDIMS, XElement DATA) 
        {
            showGrid = false;

            slotDims = SLOTDIMS;

            physicalStartPos = new Vector2((int)STARTPOS.X, (int)STARTPOS.Y);//casting to int just for safety, in this case it does matter rare instance where decimals cause issues
            totalPhysicalDims = new Vector2((int)TOTALDIMS.X, (int)TOTALDIMS.Y);

            currentHoverSlot = new Vector2(-1, -1);//not on the grid, so it is basically saying you are not hovering over anything

            gridImg = new Basic2d("2d\\UI\\HealtBar2", slotDims / 2, new Vector2(slotDims.X-2, slotDims.Y-2));//we want to do slotDIms/2 for the pos because each grid slots pos is actually top left, but we draw from the middle
                                                                                                          //so we want to move the pos to the middle of the slot(the image has an offset of half the slot dims)
            SetBaseGrid();                                                                                             //we do slotdims.X-2 because we actually want to see the grid, if you want to use the grid, but make it seem like 1 seamless image, dont use the -2(aka use -2 for debugging)
            LoadData(DATA);


        }


        public void Update(Vector2 OFFSET)
        {
            //you can process it every 3 frames, it is less process intensive
            currentHoverSlot = GetSlotFromPixel(new Vector2(Globals.mouse.newMousePos.X, Globals.mouse.newMousePos.Y), -OFFSET);
        }

        public virtual void AddGridItem(string PATH, Vector2 LOC)
        {
            gridItems.Add(new GridItem(PATH, GetPosFromLoc(LOC) + slotDims/2, new Vector2(slotDims.X, slotDims.Y), new Vector2(1, 1)));
            GetSlotFromLocation(LOC).SetToFilled(true);
        }

        public virtual Vector2 GetPosFromLoc(Vector2 LOC)//loc should be a slot or whatever
        {
            return physicalStartPos + new Vector2((int)LOC.X * slotDims.X, (int)LOC.Y * slotDims.Y);//get top left corner
        }

        public virtual GridLocation GetSlotFromLocation(Vector2 LOC)
        {
            if (LOC.X >= 0 && LOC.Y >= 0 && LOC.X < slots.Count && LOC.Y < slots[(int)LOC.X].Count)//if loc is -1 we dont want to return something
                                                                                                   //in a list you can have jagged rows, so this means that one row can be 5 slots, other row 2 slots. So that is why we error check for the slotscount
            {
                return slots[(int)LOC.X][(int)LOC.Y];//it is looking for an int, so make sure to cast it here to int aswell
                                                     //it is a 2 dimentional list(list in a list), so we use 2 brackets [][] x and y
            }
            return null;
        }

        public virtual Vector2 GetSlotFromPixel(Vector2 PIXEL, Vector2 OFFSET)//PIXEL Could be your mouse, or a loc of a mob or a location 50 px above your heros head
        {

            Vector2 adjustedPos = PIXEL - physicalStartPos + OFFSET;//if physicalStartPos is not 0, 0 it wil have an effect
            Vector2 tempVec = new Vector2(Math.Min(Math.Max(0, (int)(adjustedPos.X/slotDims.X)), slots.Count-1), Math.Min(Math.Max(0, (int)(adjustedPos.Y / slotDims.Y)), slots[0].Count - 1));

            return tempVec;
        }


        public virtual void LoadData(XElement DATA)
        {
            if (DATA != null)
            {
                List<XElement> gridItemsList = (from t in DATA.Descendants("GridItem")
                                               select t).ToList<XElement>();



                for (int i = 0; i < gridItemsList.Count; i++)
                {
                    AddGridItem("2d\\WorldItems\\Hill", new Vector2(Convert.ToInt32(gridItemsList[i].Element("Loc").Element("x").Value, Globals.culture), Convert.ToInt32(gridItemsList[i].Element("Loc").Element("y").Value, Globals.culture)));
                }
                /**/
            }    

        }


        public virtual void SetBaseGrid()
        {
            gridDims = new Vector2((int)(totalPhysicalDims.X / slotDims.X), (int)(totalPhysicalDims.Y / slotDims.Y));//take in the total physicalDims and devide those with the size of the slots to get the number of slots

            slots.Clear();

            for (int i = 0; i < gridDims.X; i++)
            {
                slots.Add(new List<GridLocation>());

                for (int j = 0; j < gridDims.Y; j++)
                {
                    slots[i].Add(new GridLocation(1, false));
                }
            }
        }





        #region A* (A Star)

        public List<Vector2> GetPath(Vector2 START, Vector2 END, bool ALLOWDIAGNALS)
        {
            List<GridLocation> viewable = new List<GridLocation>(), used = new List<GridLocation>();

            List<List<GridLocation>> masterGrid = new List<List<GridLocation>>();//copy of our grid so we can make cahnges


            bool impassable = false;
            float cost = 1;
            for (int i = 0; i < slots.Count; i++)
            {
                masterGrid.Add(new List<GridLocation>());
                for (int j = 0; j < slots[i].Count; j++)
                {
                    impassable = slots[i][j].impassable;

                    if (slots[i][j].impassable || slots[i][j].filled)
                    {

                        if (i != (int)END.X || j != (int)END.Y)//asatr mobs can find you even if you stand on a filled grid item
                        {

                            impassable = true;
                        }
                    }

                    cost = slots[i][j].cost;

                    masterGrid[i].Add(new GridLocation(new Vector2(i, j), cost, impassable, 99999999));//Vector2 is the gridpos, not in screenspace, but in gridspace
                }
            }

            viewable.Add(masterGrid[(int)START.X][(int)START.Y]);

            while (viewable.Count > 0 && !(viewable[0].pos.X == END.X && viewable[0].pos.Y == END.Y))
            {
                TestAStarNode(masterGrid, viewable, used, END, ALLOWDIAGNALS);
            }


            List<Vector2> path = new List<Vector2>();

            if (viewable.Count > 0)
            {
                int currentViewableStart = 0;
                GridLocation currentNode = viewable[currentViewableStart];

                path.Clear();
                Vector2 tempPos;


                while (true)
                {

                    //Add the difference between the actual grid and the custom grid back in...
                    tempPos = GetPosFromLoc(currentNode.pos) + slotDims / 2;
                    path.Add(new Vector2(tempPos.X, tempPos.Y));

                    if (currentNode.pos == START)
                    {
                        break;
                    }
                    else
                    {

                        if ((int)currentNode.parent.X != -1 && (int)currentNode.parent.Y != -1)
                        {
                            if (currentNode.pos.X == masterGrid[(int)currentNode.parent.X][(int)currentNode.parent.Y].pos.X && currentNode.pos.Y == masterGrid[(int)currentNode.parent.X][(int)currentNode.parent.Y].pos.Y)
                            {
                                //Current node points to its self
                                currentNode = viewable[currentViewableStart];
                                currentViewableStart++;
                            }


                            currentNode = masterGrid[(int)currentNode.parent.X][(int)currentNode.parent.Y];//for now this is all we are using, no else or if
                        }
                        else
                        {
                            //Node is off grid...
                            currentNode = viewable[currentViewableStart];
                            currentViewableStart++;
                        }


                    }

                }

                path.Reverse();

                if (path.Count > 1)
                {
                    path.RemoveAt(0);
                }
            }

            return path;
        }

        public void TestAStarNode(List<List<GridLocation>> masterGrid, List<GridLocation> viewable, List<GridLocation> used, Vector2 end, bool ALLOWDIAGNALS)
        {

            GridLocation currentNode;
            bool up = true, down = true, left = true, right = true;

            //Above
            if (viewable[0].pos.Y > 0 && viewable[0].pos.Y < masterGrid[0].Count && !masterGrid[(int)viewable[0].pos.X][(int)viewable[0].pos.Y - 1].impassable)
            {
                currentNode = masterGrid[(int)viewable[0].pos.X][(int)viewable[0].pos.Y - 1];
                up = currentNode.impassable;
                SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, 1);
            }

            //Below
            if (viewable[0].pos.Y >= 0 && viewable[0].pos.Y + 1 < masterGrid[0].Count && !masterGrid[(int)viewable[0].pos.X][(int)viewable[0].pos.Y + 1].impassable)
            {
                currentNode = masterGrid[(int)viewable[0].pos.X][(int)viewable[0].pos.Y + 1];
                down = currentNode.impassable;
                SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, 1);
            }

            //Left
            if (viewable[0].pos.X > 0 && viewable[0].pos.X < masterGrid.Count && !masterGrid[(int)viewable[0].pos.X - 1][(int)viewable[0].pos.Y].impassable)
            {
                currentNode = masterGrid[(int)viewable[0].pos.X - 1][(int)viewable[0].pos.Y];
                left = currentNode.impassable;
                SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, 1);
            }

            //Right
            if (viewable[0].pos.X >= 0 && viewable[0].pos.X + 1 < masterGrid.Count && !masterGrid[(int)viewable[0].pos.X + 1][(int)viewable[0].pos.Y].impassable)
            {
                currentNode = masterGrid[(int)viewable[0].pos.X + 1][(int)viewable[0].pos.Y];
                right = currentNode.impassable;
                SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, 1);
            }

            if (ALLOWDIAGNALS)
            {
                // Up and Right
                if (viewable[0].pos.X >= 0 && viewable[0].pos.X + 1 < masterGrid.Count && viewable[0].pos.Y > 0 && viewable[0].pos.Y < masterGrid[0].Count && !masterGrid[(int)viewable[0].pos.X + 1][(int)viewable[0].pos.Y - 1].impassable && (!up || !right))//!up || !right change this to && to allow vertical as well as diagonal lines
                {
                    currentNode = masterGrid[(int)viewable[0].pos.X + 1][(int)viewable[0].pos.Y - 1];

                    SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, (float)Math.Sqrt(2));
                }

                //Down and Right
                if (viewable[0].pos.X >= 0 && viewable[0].pos.X + 1 < masterGrid.Count && viewable[0].pos.Y >= 0 && viewable[0].pos.Y + 1 < masterGrid[0].Count && !masterGrid[(int)viewable[0].pos.X + 1][(int)viewable[0].pos.Y + 1].impassable && (!down || !right))
                {
                    currentNode = masterGrid[(int)viewable[0].pos.X + 1][(int)viewable[0].pos.Y + 1];

                    SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, (float)Math.Sqrt(2));
                }

                //Down and Left
                if (viewable[0].pos.X > 0 && viewable[0].pos.X < masterGrid.Count && viewable[0].pos.Y >= 0 && viewable[0].pos.Y + 1 < masterGrid[0].Count && !masterGrid[(int)viewable[0].pos.X - 1][(int)viewable[0].pos.Y + 1].impassable && (!down || !left))
                {
                    currentNode = masterGrid[(int)viewable[0].pos.X - 1][(int)viewable[0].pos.Y + 1];

                    SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, (float)Math.Sqrt(2));
                }

                // Up and Left
                if (viewable[0].pos.X > 0 && viewable[0].pos.X < masterGrid.Count && viewable[0].pos.Y > 0 && viewable[0].pos.Y < masterGrid[0].Count && !masterGrid[(int)viewable[0].pos.X - 1][(int)viewable[0].pos.Y - 1].impassable && (!up || !left))
                {
                    currentNode = masterGrid[(int)viewable[0].pos.X - 1][(int)viewable[0].pos.Y - 1];

                    SetAStarNode(viewable, used, currentNode, new Vector2(viewable[0].pos.X, viewable[0].pos.Y), viewable[0].currentDist, end, (float)Math.Sqrt(2));
                }
            }


            viewable[0].hasBeenUsed = true;
            used.Add(viewable[0]);
            viewable.RemoveAt(0);



            // sort
            /*viewable.Sort(delegate(AStarNode n1, AStarNode n2)
            {
                return n1.FScore.CompareTo(n2.FScore);
            });*/
        }

        public void SetAStarNode(List<GridLocation> viewable, List<GridLocation> used, GridLocation nextNode, Vector2 nextParent, float d, Vector2 target, float DISTMULT)
        {
            float f = d;
            float addedDist = (nextNode.cost * DISTMULT);




            //Add item
            if (!nextNode.isViewable && !nextNode.hasBeenUsed)
            {
                //viewable.Add(new AStarNode(nextParent, f, new Vector2(nextNode.Pos.X, nextNode.Pos.Y), nextNode.CurrentDist + 1, nextNode.Cost, nextNode.Impassable));

                nextNode.SetNode(nextParent, f, d + addedDist);
                nextNode.isViewable = true;

                SetAStarNodeInsert(viewable, nextNode);
            }
            //Node is in viewable, so check if Fscore needs revised
            else if (nextNode.isViewable)
            {

                if (f < nextNode.fScore)
                {
                    nextNode.SetNode(nextParent, f, d + addedDist);
                }
            }
        }

        public virtual void SetAStarNodeInsert(List<GridLocation> LIST, GridLocation NEWNODE)//this should use mergesort or quicksort
        {
            bool added = false;
            for (int i = 0; i < LIST.Count; i++)
            {
                if (LIST[i].fScore > NEWNODE.fScore)
                {
                    //Cant insert at 0, because that would take up the looking at node...
                    LIST.Insert(Math.Max(1, i), NEWNODE);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                LIST.Add(NEWNODE);
            }
        }


        #endregion





        public virtual void DrawGrid(Vector2 OFFSET)
        {
            if (showGrid) //if we are displaying the grid
            {
                //Vector2 topLeft = GetSlotFromPixel((new Vector2(0, 0)) / Globals.zoom  - OFFSET, Vector2.Zero);
                //Vector2 botRight = GetSlotFromPixel((new Vector2(Globals.screenWidth, Globals.screenHeight)) / Globals.zoom  - OFFSET, Vector2.Zero);
                Vector2 topLeft = GetSlotFromPixel(new Vector2(0, 0), Vector2.Zero);//top left of your screen
                Vector2 botRight = GetSlotFromPixel(new Vector2(Globals.screenWidth, Globals.screenHeight), Vector2.Zero);//bottom right of your screen

                Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                Globals.normalEffect.CurrentTechnique.Passes[0].Apply();

                for (int j = (int)topLeft.X; j <= botRight.X && j < slots.Count; j++)//we only want to draw the grid that can fit on this screen, if your screen is 1000x1000 we dont want to update the grid outside of te screen
                {
                    for (int k = (int)topLeft.Y; k <= botRight.Y && k < slots[0].Count; k++)
                    {
                        if (currentHoverSlot.X == j && currentHoverSlot.Y == k)
                        {
                            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.Red.ToVector4());//shade it to red when hovered
                            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();

                        }
                        else if (slots[j][k].filled)
                        {
                            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.Orange.ToVector4());//shade it to red when hovered
                            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();
                        }
                        else
                        {
                            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();
                        }

                        gridImg.Draw(OFFSET + physicalStartPos + new Vector2(j * slotDims.X, k * slotDims.Y));
                    }
                }
            }
            for (int i = 0; i < gridItems.Count; i++)
            {
                gridItems[i].Draw(OFFSET);
            }


        }
    }

}

