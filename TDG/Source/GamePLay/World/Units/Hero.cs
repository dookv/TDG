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
using ZeEngine;
#endregion

namespace TDG
{
    public class Hero : Unit
    {
        public string name;
        /* public Skill currentSkill;
         public List<Skill> skills = new List<Skill>();*/
        public SkillBar skillBar;
        public int zFloor = 0;
        public float zPos = 0;
        public float zSpeed = 2;
        public float zGrav = 0;
        bool zJump = false;
        public string idleAnimation = "Stand";


        public Vector2 characterVelocity;
        public float jumpForce = -5f;  // Negative value for upward force
        public float maxJumpHeight = 200f;
        public bool isJumping = false;
        public bool jumpCooldown = false;
        public float jumpStartY;

        //the vars for the working z axis jump - hsp & vsp
        public float jumpSpeed;
        public float gravSpeed;
        public float hsp;
        public float vsp;
        public float zsp;
        public float z;
        public float floorZ;

        public Hero(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, int OWNERID)
            : base(PATH, POS, DIMS, FRAMES, OWNERID)
        {
            z = 0;
            floorZ = 0;
            hsp = 0;
            vsp = 0;
            zsp = 0;
            jumpSpeed = 13;
            gravSpeed = 1;
            characterVelocity = Vector2.Zero;
            name = "Bones";
            speed = 4.0f;
            health = 500;
            healthMax = health;
            frameAnimations = true;
            currentAnimation = 0;
            frameAnimationList.Add(new FrameAnimation(new Vector2(frameSize.X, frameSize.Y), frames, new Vector2(0, 0), 4, 133, 0, "Walk"));//66 ms is 15 frames per second
            frameAnimationList.Add(new FrameAnimation(new Vector2(frameSize.X, frameSize.Y), frames, new Vector2(0, 0), 1, 133, 0, "Stand"));//66 ms is 15 frames per second
            frameAnimationList.Add(new FrameAnimation(new Vector2(frameSize.X, frameSize.Y), frames, new Vector2(0, 1), 4, 133, 0, 3, ResetZAxis, "alternate"));//66 ms is 15 frames per second
            skills.Add(new FlameWave(this));
            skills.Add(new Blink(this));
            skillBar = new SkillBar(new Vector2(30, Globals.screenHeight - 140), 52.0f, 10);

            for (int i = 0; i < skills.Count; i++)
            {
                if (i < skillBar.slots.Count)
                {
                    skillBar.slots[i].skillButton = new SkillButton("2d\\UI\\HealtBar2", new Vector2(0, 0), new Vector2(40, 40), new Vector2(1, 1), SetSkill, skills[i]);
                }
                else
                {
                    break;
                }
            }


            for (int i = 0; i < 24; i++)
            {
                inventorySlots.Add(new InventorySlot(new Vector2(0, 0), new Vector2(48, 48)));
            }

        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
            bool checkScroll = false;
            var animation = idleAnimation;
            var keyboardState = Keyboard.GetState();
            //TODO add logic so the chaarcter wont stand still if A and D are pressed simultaneously
            if (Globals.keyboard.GetPress(GameGlobals.keyBinds.GetKeyByName("Move Up")))
            {
                pos = new Vector2(pos.X, pos.Y - speed); //y in actually inverted, so y down is + and y up is -. in this case moving the character up == y - 1px
                animation = "alternate";
                checkScroll = true;

            }
            if (Globals.keyboard.GetPress(GameGlobals.keyBinds.GetKeyByName("Move Down")))
            {
                pos = new Vector2(pos.X, pos.Y + speed);
                animation = "alternate";
                checkScroll = true;
            }
            if (Globals.keyboard.GetPress(GameGlobals.keyBinds.GetKeyByName("Move Left")))
            {
                pos = new Vector2(pos.X - speed, pos.Y);

                animation = "Walk";
                checkScroll = true;
            }
            if (Globals.keyboard.GetPress(GameGlobals.keyBinds.GetKeyByName("Move Right")))
            {
                pos = new Vector2(pos.X + speed, pos.Y);
                animation = "Walk";
                checkScroll = true;
            }


            //sine wave z-axis??? cool movement
            if (Globals.keyboard.GetSinglePress("Space") && !jumpCooldown)
            {
                zsp = -jumpSpeed;
                jumpCooldown = true;
            }
            if (zsp < 0)
            {
                zsp += gravSpeed;
                pos = new Vector2(pos.X, pos.Y + z);
                if (zsp >= 0)
                {
                    isJumping = true;
                    //ResetZAxis();
                }
            }
            if (isJumping)
            {
                zsp += gravSpeed;
                pos = new Vector2(pos.X, pos.Y + z);
                if (zsp == jumpSpeed)
                {
                    jumpCooldown = false;
                    isJumping = false;
                    //ResetZAxis();
                }
            }
            z = zsp;

            /*
                        if (z + zsp > floorZ)
                        {
                            zsp = 0;
                            z = floorZ;
                        }*/

            #region
            /*            if (Globals.keyboard.GetSinglePress("Space") && !isJumping)
                        {
                            // Record the character's starting Y position for the jump.
                            jumpStartY = pos.Y;

                            // Apply an upward velocity to simulate the jump on the Z-axis.
                            characterVelocity.Y = jumpForce;

                            // Set the jumping flag to true and switch to the jump animation.
                            isJumping = true;

                        }

                        // Check if the character has reached the desired jump height.
                        if (isJumping && pos.Y <= jumpStartY - maxJumpHeight)
                        {
                            // Limit the character's upward movement to the desired jump height.
                            characterVelocity.Y = 0f;
                        }

                        // Check if the character has landed and reset the jump state.
                        if (isJumping && pos.Y >= jumpStartY)
                        {
                            isJumping = false;

                        }*/

            // Update character position
            pos += characterVelocity;

            #endregion
            #region firsthjump
            /* if (Globals.keyboard.GetSinglePress("Space"))
             {
                 if (zPos <= zFloor)
                 {
                     zJump = true;

                 }
             }

             if (zJump == true)
             {
                 zPos += zSpeed;
                 pos = new Vector2(pos.X, pos.Y + zPos);
             }

             if (zPos <= zFloor)
             {
                 zPos -= zGrav;
                 zGrav += 0.2f;
             }

             if (zPos <= zFloor + 1)
             {
                 zPos = zFloor;
                 zGrav = 0;
                 zJump = false;
             }*/
            #endregion
            #region
            /*if (zJump == true)
            {
                zPos += zSpeed;
                pos = new Vector2(pos.X, pos.Y - zPos);
                if (zPos >= zFloor)
                {


                    zPos -= zGrav;
                    zGrav += 0.2f;
                    pos = new Vector2(pos.X, pos.Y + zPos);
                    if (zPos >= 10)
                    {
                        *//*                        float i = 1;
                                                zPos += 0.15f * 1;*//*
                        pos = new Vector2(pos.X, pos.Y + 20);
                    }

                    //zJump = false;
                }

            }
            *//*

                        if (zPos <= zFloor)
                        {
                            zPos -= zGrav;
                            zGrav += 0.2f;
                            //pos = new Vector2(pos.X, pos.Y + zPos);
                        }*//*

            if (zPos <= zFloor + 1)
            {
                zPos = zFloor;
                zGrav = 0;
                zJump = false;
            }*/
            #endregion


            /*            if (Globals.keyboard.GetSinglePress("D1"))//D1 is the number 1
                        {
                            currentSkill = skills[0];
                            currentSkill.Active = true;//we can only acces the property here (Active)
                        }

                        if (Globals.keyboard.GetSinglePress("D2"))//D1 is the number 1
                        {
                            currentSkill = skills[1];
                            currentSkill.Active = true;//we can only acces the property here (Active)
                        }*/


            rot = Globals.RotateTowards(pos, new Vector2(Globals.mouse.newMouse.X, Globals.mouse.newMouse.Y) - OFFSET);//we put the hero rotation in update because it needs to be constantly updated
            /* rot = Math.Atan2(Globals.mouse.newMouse.X, Globals.mouse.newMousePos.Y);*/


            if (currentSkill == null)
            {
                if (Globals.mouse.LeftClick())
                {
                    GameGlobals.PassProjectile(new Fireball(new Vector2(pos.X, pos.Y), this, new Vector2(Globals.mouse.newMouse.X, Globals.mouse.newMouse.Y) - OFFSET));//we HAVE to make a new vector2 because if we were to use the already updating players position,                                                                                                                                                
                                                                                                                                                                        //everytime we update the fireball, the player will move.
                                                                                                                                                                        //this means this class. So in this context it is used for the OWNER (input parameter) of projectile2d. we set the owner of fireball to this class(the hero)
                    Globals.soundControl.PlaySound("Shoot");
                }
            }
            else
            {
                currentSkill.Update(OFFSET, ENEMY);

                if (currentSkill.done)
                {
                    currentSkill.Reset();
                    currentSkill = null;
                }
            }


            if (Globals.mouse.RightClick())
            {
                if (currentSkill != null)
                {
                    currentSkill.targetEffect.done = true;
                    currentSkill.Reset();
                    currentSkill = null;
                }

            }
            GameGlobals.CheckScroll(pos);//check screen scrolling
            if (checkScroll)
            {

                /* GameGlobals.CheckScroll(pos);//check if the screen needs to scroll on every movement press*/
                SetAnimationByName(animation);
            }
            else
            {
                SetAnimationByName("Stand");
            }


            skillBar.Update(Vector2.Zero);
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public virtual void ResetZAxis(Object INFO)
        {
            zsp = 0;
            pos = new Vector2(pos.X + 100, pos.Y);
            z = 0;
        }

        public virtual void SetSkill(object INFO)
        {

            SkillSelectionTypePacket tempPacket = (SkillSelectionTypePacket)INFO;
            if (INFO != null)
            {
                currentSkill = tempPacket.skill;
                currentSkill.Active = true;//we can only acces the property here (Active)
                currentSkill.selectionType = tempPacket.selectionType;
            }
        }

        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {

            base.Draw(OFFSET);
            skillBar.Draw(Vector2.Zero);
        }
    }
}
