﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Tests
{
    internal class CarBuildScreen : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D screenRender;

        RenderTarget2D carRender;
        Texture2D carSpriteSheet;

        SpriteFont font;
        string debugInfo;

        public CarBuildScreen() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            this.IsFixedTimeStep = true;
            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            this.graphics.PreferredBackBufferWidth = 640 * 2;
            this.graphics.PreferredBackBufferHeight = 360 * 2;
            graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenRender = new RenderTarget2D(GraphicsDevice, 640, 360);
            carRender = new RenderTarget2D(GraphicsDevice, 144, 80);

            font = Content.Load<SpriteFont>("Fonts/Arial12");
            carSpriteSheet = Content.Load<Texture2D>("Textures/Car");

            Animations = new Dictionary<CarAnimationType, CarAnimation>();
            animationFrame = 0;
            animationTime = 0f;

            //Running Animation
            CreateAnimations();
            selectedAnim = CarAnimationType.Running;

            animationFrame = 0;
            animationTime = 0;

            GenerateTimeLine();
            PartXSite = new Dictionary<CarPart, IPartFrame>();
            PartXSite.Add(CarPart.Body, null);
            PartXSite.Add(CarPart.BodyBack, null);
            PartXSite.Add(CarPart.BodyDown, null);
            PartXSite.Add(CarPart.Door, null);
            PartXSite.Add(CarPart.Motor, null);
            PartXSite.Add(CarPart.Wheel1, null);
            PartXSite.Add(CarPart.Wheel2, null);
            PartXSite.Add(CarPart.Wheel3, null);
            PartXSite.Add(CarPart.Wheel4, null);
            PartXSite.Add(CarPart.BackSeat, null);
            PartXSite.Add(CarPart.FrontSeat1, null);
            PartXSite.Add(CarPart.FrontSeat2, null);

            PartXSite[CarPart.Body] = CurrentAnimation.Frames[animationFrame].FramesxPart[CarPart.Body];
            PartXSite[CarPart.BodyDown] = CurrentAnimation.Frames[animationFrame].FramesxPart[CarPart.BodyDown];
            UpdatePartXSite();

            base.LoadContent();
        }
        private void CreateAnimations()
        {
            //Exportar en un XML o Json en un futuro (esperemos que no muy lejano)

            //Running
            CarAnimation RunningAnimation = new CarAnimation();
            RunningAnimation.Frames = new NewCarAnimationFrame[1];
            RunningAnimation.Frames[0] = new NewCarAnimationFrame();
            RunningAnimation.Frames[0].Duration = 16f;
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(0, 0, 144, 64)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 18, 96, 48), new Rectangle(0, 352, 96, 48)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 43, 144, 32), new Rectangle(0, 320, 144, 32)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 24, 48, 48), new Rectangle(0, 464, 48, 48)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464, 400, 48, 32)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(17, 28, 32, 32), new Rectangle(0, 400, 32, 32)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            RunningAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));


            //Running shine
            CarAnimation RunningShineAnimation = new CarAnimation();
            RunningShineAnimation.Loop = true;
            RunningShineAnimation.Frames = new NewCarAnimationFrame[6];
            RunningShineAnimation.Frames[0] = new NewCarAnimationFrame();
            RunningShineAnimation.Frames[0].Duration = 1600f;
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(0, 0, 144, 64)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 18, 96, 48), new Rectangle(0, 352, 96, 48)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 43, 144, 32), new Rectangle(0, 320, 144, 32)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 24, 48, 48), new Rectangle(0, 464, 48, 48)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464, 400, 48, 32)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(17, 28, 32, 32), new Rectangle(0, 400, 32, 32)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            RunningShineAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));

            RunningShineAnimation.Frames[1] = new NewCarAnimationFrame();
            RunningShineAnimation.Frames[1].Duration = 120f;
            RunningShineAnimation.Frames[1].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(144 * 1, 0, 144, 64)));
            RunningShineAnimation.Frames[1].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464 + 48 * 1, 400, 48, 32)));

            RunningShineAnimation.Frames[2] = new NewCarAnimationFrame();
            RunningShineAnimation.Frames[2].Duration = 80f;
            RunningShineAnimation.Frames[2].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(144 * 2, 0, 144, 64)));
            RunningShineAnimation.Frames[2].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464 + 48 * 2, 400, 48, 32)));

            RunningShineAnimation.Frames[3] = new NewCarAnimationFrame();
            RunningShineAnimation.Frames[3].Duration = 60f;
            RunningShineAnimation.Frames[3].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(144 * 3, 0, 144, 64)));
            RunningShineAnimation.Frames[3].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464 + 48 * 3, 400, 48, 32)));

            RunningShineAnimation.Frames[4] = new NewCarAnimationFrame();
            RunningShineAnimation.Frames[4].Duration = 60f;
            RunningShineAnimation.Frames[4].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(144 * 4, 0, 144, 64)));
            RunningShineAnimation.Frames[4].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464 + 48 * 4, 400, 48, 32)));

            RunningShineAnimation.Frames[5] = new NewCarAnimationFrame();
            RunningShineAnimation.Frames[5].Duration = 60f;
            RunningShineAnimation.Frames[5].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(144 * 5, 0, 144, 64)));
            RunningShineAnimation.Frames[5].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464 + 48 * 5, 400, 48, 32)));


            //Turn Left
            CarAnimation TurningLeftAnimation = new CarAnimation();
            TurningLeftAnimation.Frames = new NewCarAnimationFrame[2];
            TurningLeftAnimation.Frames[0] = new NewCarAnimationFrame();
            TurningLeftAnimation.Frames[0].Duration = 128;
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 9, 144, 64), new Rectangle(0, 64, 144, 64)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 41, 144, 32), new Rectangle(0, 320, 144, 32)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 17, 96, 48), new Rectangle(96, 352, 96, 48)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 22, 48, 48), new Rectangle(0, 464, 48, 48)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(464, 432, 48, 32)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));
            TurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(15, 26, 32, 32), new Rectangle(32, 400, 32, 32)));
            
            TurningLeftAnimation.Frames[1] = new NewCarAnimationFrame();
            TurningLeftAnimation.Frames[1].Duration = 128;
            TurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 8, 144, 64), new Rectangle(144 * 1, 64, 144, 64)));
            TurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 21, 48, 48), new Rectangle(0, 464, 48, 48)));
            TurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 40, 144, 32), new Rectangle(0, 320, 144, 32)));
            TurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 16, 96, 48), new Rectangle(96, 352, 96, 48)));
            TurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(512, 432, 48, 32)));


            //Return From Left
            CarAnimation ReturnFromLeft = new CarAnimation();
            ReturnFromLeft.Frames = new NewCarAnimationFrame[2];
            ReturnFromLeft.Frames[0] = new NewCarAnimationFrame();
            ReturnFromLeft.Frames[0].Duration = 128;
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 8, 144, 64), new Rectangle(144 * 1, 64, 144, 64)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 40, 144, 32), new Rectangle(0, 320, 144, 32)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 15, 96, 48), new Rectangle(0, 352, 96, 48)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 21, 48, 48), new Rectangle(0, 464, 48, 48)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(512, 432, 48, 32)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));
            ReturnFromLeft.Frames[0].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(15, 26, 32, 32), new Rectangle(32, 400, 32, 32)));

            ReturnFromLeft.Frames[1] = new NewCarAnimationFrame();
            ReturnFromLeft.Frames[1].Duration = 128;
            ReturnFromLeft.Frames[1].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 9, 144, 64), new Rectangle(0, 64, 144, 64)));
            ReturnFromLeft.Frames[1].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 41, 144, 32), new Rectangle(0, 320, 144, 32)));
            ReturnFromLeft.Frames[1].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 16, 96, 48), new Rectangle(0, 352, 96, 48)));
            ReturnFromLeft.Frames[1].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 22, 48, 48), new Rectangle(0, 464, 48, 48)));
            ReturnFromLeft.Frames[1].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(464, 432, 48, 32)));


            //Super Turn Left
            CarAnimation SuperTurningLeftAnimation = new CarAnimation();
            SuperTurningLeftAnimation.Frames = new NewCarAnimationFrame[4];
            SuperTurningLeftAnimation.Frames[0] = new NewCarAnimationFrame();
            SuperTurningLeftAnimation.Frames[0].Duration = 128;
            SuperTurningLeftAnimation.Frames[0] = new NewCarAnimationFrame();
            SuperTurningLeftAnimation.Frames[0].Duration = 128;
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 9, 144, 64), new Rectangle(0, 64, 144, 64)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 41, 144, 32), new Rectangle(0, 320, 144, 32)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 17, 96, 48), new Rectangle(96, 352, 96, 48)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 22, 48, 48), new Rectangle(0, 464, 48, 48)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(464, 432, 48, 32)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));
            SuperTurningLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(15, 26, 32, 32), new Rectangle(32, 400, 32, 32)));

            SuperTurningLeftAnimation.Frames[1] = new NewCarAnimationFrame();
            SuperTurningLeftAnimation.Frames[1].Duration = 32;
            SuperTurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 8, 144, 64), new Rectangle(144 * 1, 64, 144, 64)));
            SuperTurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 40, 144, 32), new Rectangle(0, 320, 144, 32)));
            SuperTurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 16, 96, 48), new Rectangle(96, 352, 96, 48)));
            SuperTurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 21, 48, 48), new Rectangle(0, 464, 48, 48)));
            SuperTurningLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(512, 432, 48, 32)));

            SuperTurningLeftAnimation.Frames[2] = new NewCarAnimationFrame();
            SuperTurningLeftAnimation.Frames[2].Duration = 32;
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 8, 144, 64), new Rectangle(144 * 2, 64, 144, 64)));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 42, 144, 32), new Rectangle(144, 320, 144, 32)));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(10, 8, 96, 48), new Rectangle(288, 352, 96, 48)));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 9, 48, 48), new Rectangle(48, 464, 48, 48)));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(87, 18, 48, 32), new Rectangle(560, 432, 48, 32)));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 44, 32, 32), new Rectangle(64, 432, 32, 32), new Rectangle(96, 432, 32, 32)));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 44, 32, 32), new Rectangle(64, 432, 32, 32), new Rectangle(96, 432, 32, 32)));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(18, 48, 32, 32), new Rectangle(192, 432, 32, 32), new Rectangle(224, 432, 32, 32), false, true));
            SuperTurningLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(99, 48, 32, 32), new Rectangle(192, 432, 32, 32), new Rectangle(224, 432, 32, 32), false, true));

            SuperTurningLeftAnimation.Frames[3] = new NewCarAnimationFrame();
            SuperTurningLeftAnimation.Frames[3].Duration = 64;
            SuperTurningLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 5, 144, 64), new Rectangle(144 * 3, 64, 144, 64)));
            SuperTurningLeftAnimation.Frames[3].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 39, 144, 32), new Rectangle(144 * 2, 320, 144, 32)));
            SuperTurningLeftAnimation.Frames[3].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(10, 6, 96, 48), new Rectangle(288, 352, 96, 48)));
            SuperTurningLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 6, 48, 48), new Rectangle(48, 464, 48, 48)));
            SuperTurningLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(87, 15, 48, 32), new Rectangle(560, 432, 48, 32)));
            SuperTurningLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 41, 32, 32), new Rectangle(64, 432, 32, 32), new Rectangle(96, 432, 32, 32), true));
            SuperTurningLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 41, 32, 32), new Rectangle(64, 432, 32, 32), new Rectangle(96, 432, 32, 32), true));


            //Return from Super Left
            CarAnimation ReturnFromSuperLeftAnimation = new CarAnimation();
            ReturnFromSuperLeftAnimation.Frames = new NewCarAnimationFrame[5];
            ReturnFromSuperLeftAnimation.Frames[0] = new NewCarAnimationFrame();
            ReturnFromSuperLeftAnimation.Frames[0].Duration = 80;
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 8, 144, 64), new Rectangle(144 * 2, 64, 144, 64)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 42, 144, 32), new Rectangle(144, 320, 144, 32)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(10, 8, 96, 48), new Rectangle(288, 352, 96, 48)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 9, 48, 48), new Rectangle(48, 464, 48, 48)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(87, 18, 48, 32), new Rectangle(560, 432, 48, 32)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 44, 32, 32), new Rectangle(64, 432, 32, 32), new Rectangle(96, 432, 32, 32)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 44, 32, 32), new Rectangle(64, 432, 32, 32), new Rectangle(96, 432, 32, 32)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(18, 48, 32, 32), new Rectangle(192, 432, 32, 32), new Rectangle(224, 432, 32, 32), false, true));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(99, 48, 32, 32), new Rectangle(192, 432, 32, 32), new Rectangle(224, 432, 32, 32), false, true));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));
            ReturnFromSuperLeftAnimation.Frames[0].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(15, 26, 32, 32), new Rectangle(32, 400, 32, 32)));

            ReturnFromSuperLeftAnimation.Frames[1] = new NewCarAnimationFrame();
            ReturnFromSuperLeftAnimation.Frames[1].Duration = 48;
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 8, 144, 64), new Rectangle(144 * 1, 64, 144, 64)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 40, 144, 32), new Rectangle(0, 320, 144, 32)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 15, 96, 48), new Rectangle(0, 352, 96, 48)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 21, 48, 48), new Rectangle(0, 464, 48, 48)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(512, 432, 48, 32)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Wheel1, new WheelFrame(new Rectangle(9, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Wheel2, new WheelFrame(new Rectangle(89, 54, 32, 32), new Rectangle(0, 432, 32, 32), new Rectangle(32, 432, 32, 32)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Wheel3, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false, false));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.Wheel4, new WheelFrame(new Rectangle(), new Rectangle(), new Rectangle(), false, false));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));
            ReturnFromSuperLeftAnimation.Frames[1].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(15, 26, 32, 32), new Rectangle(32, 400, 32, 32)));

            ReturnFromSuperLeftAnimation.Frames[2] = new NewCarAnimationFrame();
            ReturnFromSuperLeftAnimation.Frames[2].Duration = 48;
            ReturnFromSuperLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 9, 144, 64), new Rectangle(0, 64, 144, 64)));
            ReturnFromSuperLeftAnimation.Frames[2].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 41, 144, 32), new Rectangle(0, 320, 144, 32)));
            ReturnFromSuperLeftAnimation.Frames[2].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 16, 96, 48), new Rectangle(0, 352, 96, 48)));
            ReturnFromSuperLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 22, 48, 48), new Rectangle(0, 464, 48, 48)));
            ReturnFromSuperLeftAnimation.Frames[2].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 23, 48, 32), new Rectangle(464, 432, 48, 32)));

            ReturnFromSuperLeftAnimation.Frames[3] = new NewCarAnimationFrame();
            ReturnFromSuperLeftAnimation.Frames[3].Duration = 80;
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 11, 144, 64), new Rectangle(0, 0, 144, 64)));
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 18, 96, 48), new Rectangle(0, 352, 96, 48)));
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 43, 144, 32), new Rectangle(0, 320, 144, 32)));
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 24, 48, 48), new Rectangle(0, 464, 48, 48)));
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 24, 48, 32), new Rectangle(464, 400, 48, 32)));
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.BackSeat, new PartFrame(new Rectangle(17, 28, 32, 32), new Rectangle(0, 400, 32, 32)));
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.FrontSeat1, new PartFrame(new Rectangle(64, 25, 16, 32), new Rectangle(160, 400, 16, 32)));
            ReturnFromSuperLeftAnimation.Frames[3].FramesxPart.Add(CarPart.FrontSeat2, new PartFrame(new Rectangle(51, 29, 16, 32), new Rectangle(160, 400, 16, 32)));

            ReturnFromSuperLeftAnimation.Frames[4] = new NewCarAnimationFrame();
            ReturnFromSuperLeftAnimation.Frames[4].Duration = 80;
            ReturnFromSuperLeftAnimation.Frames[4].FramesxPart.Add(CarPart.Body, new PartFrame(new Rectangle(0, 12, 144, 64), new Rectangle(0, 128, 144, 64)));
            ReturnFromSuperLeftAnimation.Frames[4].FramesxPart.Add(CarPart.BodyDown, new PartFrame(new Rectangle(0, 42, 144, 32), new Rectangle(0, 320, 144, 32)));
            ReturnFromSuperLeftAnimation.Frames[4].FramesxPart.Add(CarPart.BodyBack, new PartFrame(new Rectangle(14, 19, 96, 48), new Rectangle(192, 352, 96, 48)));
            ReturnFromSuperLeftAnimation.Frames[4].FramesxPart.Add(CarPart.Door, new PartFrame(new Rectangle(46, 25, 48, 48), new Rectangle(0, 464, 48, 48)));
            ReturnFromSuperLeftAnimation.Frames[4].FramesxPart.Add(CarPart.Motor, new PartFrame(new Rectangle(85, 25, 48, 32), new Rectangle(464, 464, 48, 32)));


            Animations.Add(CarAnimationType.Running, RunningAnimation);
            Animations.Add(CarAnimationType.Running | CarAnimationType.Shine, RunningShineAnimation);
            Animations.Add(CarAnimationType.TurningLeft, TurningLeftAnimation);
            Animations.Add(CarAnimationType.TurningLeft | CarAnimationType.ReturnToRunning, ReturnFromLeft);
            Animations.Add(CarAnimationType.SuperTurningLeft, SuperTurningLeftAnimation);
            Animations.Add(CarAnimationType.SuperTurningLeft | CarAnimationType.ReturnToRunning, ReturnFromSuperLeftAnimation);
        }


        bool playing;

        bool playingGeneral;
        int animationFrame;
        float animationTime;

        bool playingWheel;
        int wheelFrame;
        float wheelTime;

        public Dictionary<CarAnimationType, CarAnimation> Animations;
        public CarAnimation CurrentAnimation { get => Animations[selectedAnim]; }

        public Dictionary<CarPart, IPartFrame> PartXSite;
        Dictionary<float, Dictionary<CarPart, IPartFrame>> AnimTimeLine;
        Dictionary<int, TimeLineSegment> AnimTimeLine2;

        public struct TimeLineSegment
        {
            public float TotalTime;
            public Dictionary<CarPart, IPartFrame> FramesxPart;
            //public Dictionary<CarPart, bool> PartHasValue;
            public Dictionary<CarPart, int> PartFromIndex;
        }

        bool runBodyMovement;
        float runBodyMovementElapsed;
        Point carPos = Point.Zero;

        CarPart selectedPart = CarPart.Body;
        CarAnimationType selectedAnim = CarAnimationType.Running;

        bool upLastPress;
        bool downLastPress;
        bool leftLastPress;
        bool rightLastPress;
        bool addPress;
        bool susPress;
        bool spacePress;
        bool D1Press;
        bool D2Press;
        bool D3Press;
        bool D4Press;
        bool NPress;
        bool MPress;
        bool PPress;
        bool OPress;
        bool ZPress;
        bool XPress;
        bool Numpad1Press;
        bool Numpad2Press;
        bool Numpad3Press;

        bool modifyAllFrames;
        bool doMovement = true;

        int scale = 1;
        float speedMod = 1f;

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            General.GameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape))
                Exit();

            if (doMovement)
            {
                runBodyMovementElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
                if (runBodyMovementElapsed > 256)
                {
                    runBodyMovementElapsed = runBodyMovementElapsed - 256;
                    runBodyMovement = !runBodyMovement;
                }
            }

            UpdateZoom(ks);
            UpdateChangePart(ks);
            UpdateChangePosition(ks);
            UpdateManualChangeFrame(ks);
            UpdateChangeAnimation(ks);

            if (ks.IsKeyDown(Keys.P))
            {
                if (!PPress)
                    modifyAllFrames = !modifyAllFrames;

                PPress = true;
            }
            else PPress = false;

            UpdatePlayAnimation(ks);
            UpdateAnimation();

            Point pos = AnimTimeLine2[animationFrame].FramesxPart[selectedPart].Rectangle.Location;
            Point a = new Point(AnimTimeLine2[animationFrame].FramesxPart[selectedPart].Rectangle.Right, AnimTimeLine2[animationFrame].FramesxPart[selectedPart].Rectangle.Bottom);
            Point pos2 = new Point(144, 80) - a;

            debugInfo = selectedPart.ToString();
            debugInfo += "\nLeft: " + pos.X + " Top: " + pos.Y;
            debugInfo += "\nRight: " + pos2.X + " Bottom: " + pos2.Y;
            if (AnimTimeLine2[animationFrame].PartFromIndex[selectedPart] != animationFrame) debugInfo += " (From Index: " + AnimTimeLine2[animationFrame].PartFromIndex[selectedPart] + ")";
            debugInfo += "\nModify mode: " + (modifyAllFrames ? "All Frames" : "Only this Frame");
            //debugInfo += "\nShowing: " + CurrentAnimation.Frames[animationFrame].FramesxPart[selectedPart].Visible;
            debugInfo += "\n-------------";
            debugInfo += "\nAnimation: " + selectedAnim.ToString();
            debugInfo += "\nPlaying General: " + playingGeneral;
            debugInfo += "\nPlaying Wheels: " + playingWheel;
            debugInfo += "\nMovement Enable: " + doMovement;
            debugInfo += "\nFrame Index: " + animationFrame.ToString();
            debugInfo += "\nSpeed: x" + speedMod.ToString();
        }
        private void UpdateZoom(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Add))
            {
                if (scale < 5 && (ks.IsKeyDown(Keys.Add) && !addPress))
                    scale++;
                addPress = true;
            }
            else addPress = false;

            if (ks.IsKeyDown(Keys.Subtract))
            {
                if (scale > 1 && (ks.IsKeyDown(Keys.Subtract) && !susPress))
                    scale--;
                susPress = true;
            }
            else susPress = false;
        }
        private void UpdateChangePart(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.D1))
            {
                if (!D1Press)
                {
                    if (selectedPart == CarPart.Body)
                        selectedPart = CarPart.BodyBack;
                    else if (selectedPart == CarPart.BodyBack)
                        selectedPart = CarPart.BodyDown;
                    else
                        selectedPart = CarPart.Body;

                }
                D1Press = true;
            }
            else D1Press = false;

            if (ks.IsKeyDown(Keys.D2))
            {
                if (!D2Press)
                {
                    if (selectedPart == CarPart.Wheel1)
                        selectedPart = CarPart.Wheel2;
                    else if (selectedPart == CarPart.Wheel2)
                        selectedPart = CarPart.Wheel3;
                    else if (selectedPart == CarPart.Wheel3)
                        selectedPart = CarPart.Wheel4;
                    else
                        selectedPart = CarPart.Wheel1;
                }
                D2Press = true;
            }
            else D2Press = false;

            if (ks.IsKeyDown(Keys.D3))
            {
                if (!D3Press)
                {
                    if (selectedPart == CarPart.Door)
                        selectedPart = CarPart.Motor;
                    else
                        selectedPart = CarPart.Door;
                }
                D3Press = true;
            }
            else D3Press = false;

            if (ks.IsKeyDown(Keys.D4))
            {
                if (!D4Press)
                {
                    if (selectedPart == CarPart.BackSeat)
                        selectedPart = CarPart.FrontSeat1;
                    else if (selectedPart == CarPart.FrontSeat1)
                        selectedPart = CarPart.FrontSeat2;
                    else
                        selectedPart = CarPart.BackSeat;
                }
                D4Press = true;
            }
            else D4Press = false;
        }
        private void UpdateChangeAnimation(KeyboardState ks)
        {
            //if (playing) return;
            if (ks.IsKeyDown(Keys.NumPad1))
            {
                if (!Numpad1Press)
                {
                    if (selectedAnim == CarAnimationType.TurningLeft)
                    {
                        int frame = 1 - animationFrame;
                        SetAnimation(CarAnimationType.TurningLeft | CarAnimationType.ReturnToRunning, frame);
                    }
                    else if (selectedAnim == CarAnimationType.SuperTurningLeft)
                    {
                        int frame = 3 - animationFrame;
                        SetAnimation(CarAnimationType.SuperTurningLeft | CarAnimationType.ReturnToRunning, frame);
                    }
                    else if (selectedAnim == CarAnimationType.Running)
                        SetAnimation(CarAnimationType.Running | CarAnimationType.Shine);
                    else
                        SetAnimation(CarAnimationType.Running);
                }
                Numpad1Press = true;
            }
            else Numpad1Press = false;

            if (ks.IsKeyDown(Keys.NumPad2))
            {
                if (!Numpad2Press)
                {
                    if (selectedAnim == CarAnimationType.TurningLeft)
                        SetAnimation(CarAnimationType.SuperTurningLeft, animationFrame);
                    //else if (selectedAnim == CarAnimationType.TurningLeft)
                        //SetAnimation(CarAnimationType.TurningLeft | CarAnimationType.Shine, false);
                    else
                        SetAnimation(CarAnimationType.TurningLeft);
                }
                Numpad2Press = true;
            }
            else Numpad2Press = false;

            if (ks.IsKeyDown(Keys.NumPad3))
            {
                if (!Numpad3Press)
                {
                    CarAnimationType? newAnim = null;
                    if (selectedAnim == CarAnimationType.Running)
                        newAnim = CarAnimationType.SuperTurningLeft;

                    if (newAnim.HasValue) SetAnimation(newAnim.Value);
                }
                Numpad3Press = true;
            }
            else Numpad3Press = false;
        }
        private void UpdatePlayAnimation(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Space))
            {
                if (!spacePress)
                {
                    animationTime = 0f;
                    animationFrame = 0;
                    wheelTime = 0f;
                    wheelFrame = 0;

                    playing = !playing;
                    playingGeneral = playing;
                    playingWheel = playing;

                    UpdatePartXSite();
                }
                spacePress = true;
            }
            else spacePress = false;

            if (ks.IsKeyDown(Keys.O))
            {
                if (!OPress)
                {
                    doMovement = !doMovement;
                    runBodyMovement = false;
                }
                OPress = true;
            }
            else OPress = false;

            if (ks.IsKeyDown(Keys.Z))
            {
                if (!ZPress)
                {
                    if (speedMod > 0.25f)
                    {
                        if (speedMod == 4f) speedMod = 2f;
                        else if (speedMod == 2f) speedMod = 1f;
                        else if (speedMod == 1f) speedMod = 0.5f;
                        else if (speedMod == 0.5f) speedMod = 0.25f;
                    }
                }
                ZPress = true;
            }
            else ZPress = false;

            if (ks.IsKeyDown(Keys.X))
            {
                if (!XPress)
                {
                    if (speedMod < 4f)
                    {
                        if (speedMod == 0.25f) speedMod = 0.5f;
                        else if (speedMod == 0.5f) speedMod = 1f;
                        else if (speedMod == 1f) speedMod = 2f;
                        else if (speedMod == 2f) speedMod = 4f;
                    }
                }
                XPress = true;
            }
            else XPress = false;
        }

        private void UpdateChangePosition(KeyboardState ks)
        {
            if (playing) return;

            Rectangle rectangle = AnimTimeLine2[animationFrame].FramesxPart[selectedPart].Rectangle;
            int originalFrame = AnimTimeLine2[animationFrame].PartFromIndex[selectedPart];

            bool shift = ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift);

            if (ks.IsKeyDown(Keys.Up))
            {
                if ((shift && !upLastPress) || !ks.IsKeyDown(Keys.LeftShift))
                {
                    if (modifyAllFrames)
                    {
                        for (int i = 0; i < CurrentAnimation.Frames.Length; i++)
                            if (CurrentAnimation.Frames[i].FramesxPart.ContainsKey(selectedPart))
                            {
                                rectangle.Y -= 1;
                                CurrentAnimation.Frames[i].FramesxPart[selectedPart].Rectangle = rectangle;
                            }
                    }
                    else
                    {
                        rectangle.Y -= 1;
                        CurrentAnimation.Frames[originalFrame].FramesxPart[selectedPart].Rectangle = rectangle;
                    }
                    GenerateTimeLine();
                    UpdatePartXSite();
                }
                upLastPress = true;
            }
            else upLastPress = false;

            if (ks.IsKeyDown(Keys.Down))
            {
                if ((shift && !downLastPress) || !ks.IsKeyDown(Keys.LeftShift))
                {
                    if (modifyAllFrames)
                    {
                        for (int i = 0; i < CurrentAnimation.Frames.Length; i++)
                            if (CurrentAnimation.Frames[i].FramesxPart.ContainsKey(selectedPart))
                            {
                                rectangle.Y += 1;
                                CurrentAnimation.Frames[i].FramesxPart[selectedPart].Rectangle = rectangle;
                            }
                    }
                    else
                    {
                        rectangle.Y += 1;
                        CurrentAnimation.Frames[originalFrame].FramesxPart[selectedPart].Rectangle = rectangle;
                    }
                    GenerateTimeLine();
                    UpdatePartXSite();
                }
                downLastPress = true;
            }
            else downLastPress = false;

            if (ks.IsKeyDown(Keys.Left))
            {
                if ((shift && !leftLastPress) || !ks.IsKeyDown(Keys.LeftShift))
                {
                    if (modifyAllFrames)
                    {
                        for (int i = 0; i < CurrentAnimation.Frames.Length; i++)
                            if (CurrentAnimation.Frames[i].FramesxPart.ContainsKey(selectedPart))
                            {
                                rectangle.X -= 1;
                                CurrentAnimation.Frames[i].FramesxPart[selectedPart].Rectangle = rectangle;
                            }
                    }
                    else
                    {
                        rectangle.X -= 1;
                        CurrentAnimation.Frames[originalFrame].FramesxPart[selectedPart].Rectangle = rectangle;
                    }
                    GenerateTimeLine();
                    UpdatePartXSite();
                }
                leftLastPress = true;
            }
            else leftLastPress = false;

            if (ks.IsKeyDown(Keys.Right))
            {
                if ((shift && !rightLastPress) || !ks.IsKeyDown(Keys.LeftShift))
                {
                    if (modifyAllFrames)
                    {
                        for (int i = 0; i < CurrentAnimation.Frames.Length; i++)
                            if (CurrentAnimation.Frames[i].FramesxPart.ContainsKey(selectedPart))
                            {
                                rectangle.X += 1;
                                CurrentAnimation.Frames[i].FramesxPart[selectedPart].Rectangle = rectangle;
                            }
                    }
                    else
                    {
                        rectangle.X += 1;
                        CurrentAnimation.Frames[originalFrame].FramesxPart[selectedPart].Rectangle = rectangle;
                    }
                    GenerateTimeLine();
                    UpdatePartXSite();
                }
                rightLastPress = true;
            }
            else rightLastPress = false;
        }
        private void UpdateManualChangeFrame(KeyboardState ks)
        {
            if (playing) return;
            if (ks.IsKeyDown(Keys.N))
            {
                if (!NPress)
                {
                    animationFrame--;
                    if (animationFrame < 0) animationFrame = CurrentAnimation.Frames.Length - 1;
                    UpdatePartXSite();
                }
                NPress = true;
            }
            else NPress = false;

            if (ks.IsKeyDown(Keys.M))
            {
                if (!MPress)
                {
                    animationFrame++;
                    if (animationFrame > CurrentAnimation.Frames.Length - 1) animationFrame = 0;
                    UpdatePartXSite();
                }
                MPress = true;
            }
            else MPress = false;
        }

        private void SetAnimation(CarAnimationType anim, int frame = 0)
        {
            animationFrame = frame;
            animationTime = 0f;
            selectedAnim = anim;
            playingGeneral = playing;
            GenerateTimeLine();
            UpdatePartXSite();
        }
        private void UpdateAnimation()
        {
            if (!playing) return;

            if (playingGeneral)
            {
                if (animationTime >= CurrentAnimation.Frames[animationFrame].Duration)
                {
                    animationTime -= CurrentAnimation.Frames[animationFrame].Duration;
                    animationFrame++;
                    if (animationFrame > CurrentAnimation.Frames.Length - 1)
                    {
                        if (CurrentAnimation.Loop) animationFrame = 0;
                        else
                        {
                            animationFrame = CurrentAnimation.Frames.Length - 1;
                            if (selectedAnim == (CarAnimationType.TurningLeft | CarAnimationType.ReturnToRunning))
                                SetAnimation(CarAnimationType.Running);
                            else if (selectedAnim == (CarAnimationType.SuperTurningLeft | CarAnimationType.ReturnToRunning))
                                SetAnimation(CarAnimationType.Running);
                            else playingGeneral = false;
                        }
                    }
                    UpdatePartXSite();
                }
                animationTime += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
            }

            if (playingWheel)
            {
                if (wheelTime >= 128)
                {
                    wheelTime -= 128;
                    wheelFrame++;
                    if (wheelFrame > 1)
                        wheelFrame = 0;
                }
                wheelTime += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
            }
        }

        private void GenerateTimeLine()
        {
            AnimTimeLine2 = new Dictionary<int, TimeLineSegment>();
            Dictionary<CarPart, IPartFrame> updatedValues = new Dictionary<CarPart, IPartFrame>();
            Dictionary<CarPart, int> updatedIndex = new Dictionary<CarPart, int>();

            AnimTimeLine = new Dictionary<float, Dictionary<CarPart, IPartFrame>>();

            int[] vals = (int[])Enum.GetValues(typeof(CarPart));
            for (int i = 0; i < vals.Length; i++)
            {
                updatedValues.Add((CarPart)vals[i], null);
                updatedIndex.Add((CarPart)vals[i], -1);
            }

            float timeAcum = 0;
            for (int i = 0; i < CurrentAnimation.Frames.Length; i++)
            {
                updatedValues = updatedValues.ToDictionary(entry => entry.Key, entry => entry.Value);
                updatedIndex = updatedIndex.ToDictionary(entry => entry.Key, entry => entry.Value);

                for (int j = 0; j < vals.Length; j++)
                {
                    CarPart part = (CarPart)vals[j];
                    if (CurrentAnimation.Frames[i].FramesxPart.ContainsKey(part))
                    {
                        updatedIndex[part] = i;
                        updatedValues[part] = CurrentAnimation.Frames[i].FramesxPart[part];
                    }
                }

                TimeLineSegment ts = new TimeLineSegment();
                ts.TotalTime = timeAcum;
                ts.FramesxPart = updatedValues;
                ts.PartFromIndex = updatedIndex;

                AnimTimeLine2.Add(i, ts);
                timeAcum += CurrentAnimation.Frames[i].Duration;
            }
        }
        private void UpdatePartXSite()
        {
            PartXSite[CarPart.Body] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.Body];
            PartXSite[CarPart.BodyBack] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.BodyBack];
            PartXSite[CarPart.BodyDown] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.BodyDown];
            PartXSite[CarPart.Door] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.Door];
            PartXSite[CarPart.Motor] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.Motor];
            PartXSite[CarPart.Wheel1] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.Wheel1];
            PartXSite[CarPart.Wheel2] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.Wheel2];
            PartXSite[CarPart.Wheel3] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.Wheel3];
            PartXSite[CarPart.Wheel4] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.Wheel4];
            PartXSite[CarPart.BackSeat] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.BackSeat];
            PartXSite[CarPart.FrontSeat1] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.FrontSeat1];
            PartXSite[CarPart.FrontSeat2] = AnimTimeLine2[animationFrame].FramesxPart[CarPart.FrontSeat2];
        }

        private PartFrame GetPartFrame(IPartFrame partFrame) => (PartFrame)partFrame;

        protected override void Draw(GameTime gameTime)
        {
            //Render Car Sprite
            GraphicsDevice.SetRenderTarget(carRender);
            GraphicsDevice.Clear(new Color(Color.Blue, 0.2f));
            spriteBatch.Begin();

            //Wheel 4
            if (PartXSite[CarPart.Wheel4].Visible)
            {
                Rectangle r = PartXSite[CarPart.Wheel4].Rectangle;
                if (PartXSite[CarPart.Wheel4].ApplyCarMovement && runBodyMovement && playing) r.Y += 1;
                Rectangle source = wheelFrame == 0 ? ((WheelFrame)PartXSite[CarPart.Wheel4]).Source1 : ((WheelFrame)PartXSite[CarPart.Wheel4]).Source2;
                spriteBatch.Draw(carSpriteSheet, r, source, Color.White);
            }

            //Wheel 3
            if (PartXSite[CarPart.Wheel3].Visible)
            {
                Rectangle r = PartXSite[CarPart.Wheel3].Rectangle;
                if (PartXSite[CarPart.Wheel3].ApplyCarMovement && runBodyMovement && playing) r.Y += 1;
                Rectangle source = wheelFrame == 0 ? ((WheelFrame)PartXSite[CarPart.Wheel3]).Source1 : ((WheelFrame)PartXSite[CarPart.Wheel3]).Source2;
                spriteBatch.Draw(carSpriteSheet, r, source, Color.White);
            }

            //Draw BodyBack
            if (PartXSite[CarPart.BodyBack].Visible)
            {
                Rectangle r = PartXSite[CarPart.BodyBack].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.BodyBack]).Source, Color.White);
            }

            //Back Seat
            if (PartXSite[CarPart.BackSeat].Visible)
            {
                Rectangle r = PartXSite[CarPart.BackSeat].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.BackSeat]).Source, Color.White);
            }

            //Back Seat 1
            if (PartXSite[CarPart.FrontSeat1].Visible)
            {
                Rectangle r = PartXSite[CarPart.FrontSeat1].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.FrontSeat1]).Source, Color.White);
            }

            //Back Seat 2
            if (PartXSite[CarPart.FrontSeat2].Visible)
            {
                Rectangle r = PartXSite[CarPart.FrontSeat2].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.FrontSeat2]).Source, Color.White);
            }

            //Body Down
            if (PartXSite[CarPart.BodyDown].Visible)
            {
                Rectangle r = PartXSite[CarPart.BodyDown].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.BodyDown]).Source, Color.White);
            }

            //Wheel 2
            if (PartXSite[CarPart.Wheel2].Visible)
            {
                Rectangle r = PartXSite[CarPart.Wheel2].Rectangle;
                if (PartXSite[CarPart.Wheel2].ApplyCarMovement && runBodyMovement && playing) r.Y += 1;
                Rectangle source = wheelFrame == 0 ? ((WheelFrame)PartXSite[CarPart.Wheel2]).Source1 : ((WheelFrame)PartXSite[CarPart.Wheel2]).Source2;
                spriteBatch.Draw(carSpriteSheet, r, source, Color.White);
            }

            //Wheel 1
            if (PartXSite[CarPart.Wheel1].Visible)
            {
                Rectangle r = PartXSite[CarPart.Wheel1].Rectangle;
                if (PartXSite[CarPart.Wheel1].ApplyCarMovement && runBodyMovement && playing) r.Y += 1;
                Rectangle source = wheelFrame == 0 ? ((WheelFrame)PartXSite[CarPart.Wheel1]).Source1 : ((WheelFrame)PartXSite[CarPart.Wheel1]).Source2;
                spriteBatch.Draw(carSpriteSheet, r, source, Color.White);
            }

            //Draw Body
            if (PartXSite[CarPart.Body].Visible)
            {
                Rectangle r = PartXSite[CarPart.Body].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.Body]).Source, Color.White);
            }

            //Motor / Capo
            if (PartXSite[CarPart.Motor].Visible)
            {
                Rectangle r = PartXSite[CarPart.Motor].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.Motor]).Source, Color.White);
            }

            //Door
            if (PartXSite[CarPart.Door].Visible)
            {
                Rectangle r = PartXSite[CarPart.Door].Rectangle;
                if (runBodyMovement && playing) r.Y += 1;

                spriteBatch.Draw(carSpriteSheet, r, GetPartFrame(PartXSite[CarPart.Door]).Source, Color.White);
            }

            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            //Render on 640x360 px
            GraphicsDevice.SetRenderTarget(screenRender);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(carRender, new Rectangle(carPos, carRender.Bounds.Size), Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            //Render on Windows Scale
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth * scale, graphics.PreferredBackBufferHeight * scale), Color.White);
            spriteBatch.DrawString(font, debugInfo, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
