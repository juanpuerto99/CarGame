using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarGame
{
    [Flags()]
    public enum CarAnimationType : Int32
    {
        Running = 1,
        TurningLeft = 2,
        SuperTurningLeft = 4,
        TurningRight = 8,
        SuperTurningRight = 16,
        ReturnToRunning = 32,
        //Crashing
        Shine = 64,
    }
    public enum CarPart : byte
    {
        Body,
        BodyBack,
        BodyDown,

        Motor,
        Door,

        Wheel1,
        Wheel2,
        Wheel3,
        Wheel4,

        FrontSeat1,
        FrontSeat2,
        BackSeat
    }
    public class CarAnimation
    {
        public NewCarAnimationFrame[] Frames;
        public bool Loop = false;
    }
    public struct NewCarAnimationFrame
    {
        public float Duration;
        public Dictionary<CarPart, IPartFrame> FramesxPart;

        public NewCarAnimationFrame()
        {
            Duration = 0f;
            FramesxPart = new Dictionary<CarPart, IPartFrame>();
        }
        public NewCarAnimationFrame Clone() => (NewCarAnimationFrame)this.MemberwiseClone();
    }

    public interface IPartFrame
    {
        public Rectangle Rectangle { get; set; }
        public bool Visible { get; set; }
        public bool ApplyCarMovement { get; set; }

        public IPartFrame Clone();
    }
    public struct PartFrame : IPartFrame
    {
        public Rectangle Rectangle { get; set; }
        public Rectangle Source;
        public bool Visible { get; set; }
        public bool ApplyCarMovement { get; set; }

        public PartFrame()
        {
            Rectangle = Rectangle.Empty;
            Source = Rectangle.Empty;
            Visible = false;
            ApplyCarMovement = false;
        }
        public PartFrame(Rectangle pos, Rectangle source)
        {
            Rectangle = pos;
            Source = source;
            Visible = true;
            ApplyCarMovement = true;
        }
        public PartFrame(Rectangle pos, Rectangle source, bool visible)
        {
            Rectangle = pos;
            Source = source;
            Visible = visible;
            ApplyCarMovement = true;
        }

        public IPartFrame Clone() => this.MemberwiseClone() as IPartFrame;

    }
    public struct WheelFrame : IPartFrame
    {
        public Rectangle Rectangle { get; set; }
        public Rectangle Source1 { get; set; }
        public Rectangle Source2 { get; set; }
        public bool Visible { get; set; }
        public bool ApplyCarMovement { get; set; }

        public WheelFrame()
        {
            Rectangle = Rectangle.Empty;
            Source1 = Rectangle.Empty;
            Source2 = Rectangle.Empty;
            Visible = false;
            ApplyCarMovement = false;
        }
        public WheelFrame(Rectangle pos, Rectangle source1, Rectangle source2)
        {
            Rectangle = pos;
            Source1 = source1;
            Source2 = source2;
            Visible = true;
            ApplyCarMovement = false;
        }
        public WheelFrame(Rectangle pos, Rectangle source1, Rectangle source2, bool applyCarMovement, bool visible)
        {
            Rectangle = pos;
            Source1 = source1;
            Source2 = source2;
            Visible = visible;
            ApplyCarMovement = applyCarMovement;
        }
        public WheelFrame(Rectangle pos, Rectangle source1, Rectangle source2, bool applyCarMovement)
        {
            Rectangle = pos;
            Source1 = source1;
            Source2 = source2;
            ApplyCarMovement = applyCarMovement;
            Visible = true;
        }

        public IPartFrame Clone() => this.MemberwiseClone() as IPartFrame;
    }


    public class Car : Entity
    {
        private Dictionary<CarAnimationType, CarAnimation> Animations;
        private Dictionary<CarPart, IPartFrame> PartXSite;
        private CarAnimationType currentAnimation = CarAnimationType.Running;

        private int animationFrame;
        private int currentWheelSource;
        private float animationTime;
        private float wheelTime;

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < General.Obstacles.Count; i++)
            {
                if (HitBox.CheckCollision(General.Obstacles[i].HitBox))
                {
                    //General.Obstacles[i].CarCrashed();
                }
            }
        }
    }
}
