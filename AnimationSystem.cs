using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public struct AnimationFrame
    {
        public Rectangle Source;
        public float Duration;

        public AnimationFrame()
        {
            Source = new Rectangle();
            Duration = 0f;
        }
        public AnimationFrame(Rectangle source, float speed)
        {
            this.Source = source;
            this.Duration = speed;
        }
        public AnimationFrame(int x, int y, int width, int height, float speed)
        {
            this.Source = new Rectangle(x, y, width, height);
            this.Duration = speed;
        }
    }
    public class AnimationData
    {
        public string Name;
        public AnimationFrame[] Frames;

        public AnimationData()
        {
            this.Name = "Default" + GetHashCode();
            this.Frames = new AnimationFrame[0];
        }
        public AnimationData(string name, AnimationFrame[] frames)
        {
            this.Name = name;
            this.Frames = frames;
        }
    }
    [DebuggerDisplay("Name: {Name} | Frame: {FrameIndex} | State: {State}")]
    public class AnimationSystem
    {
        private AnimationData animation = null;
        public string Name 
        { 
            get
            {
                if (animation == null) return null;
                return animation.Name; 
            }
        }

        private AnimationState state;
        public AnimationState State { get => state; }

        private float frameElapsed = 0f;
        private AnimationFrame CurrentFrame { get => animation.Frames[frameIndex]; }

        public int TotalFrames { get => animation.Frames.Length; }

        private int frameIndex = 0;
        public int FrameIndex { get => frameIndex; }

        public float speedMod = 1f;
        public float SpeedMod { get => speedMod; set => speedMod = value; }

        private bool bucle = false;
        public bool Bucle { get => bucle; set => bucle = value; }

        public Rectangle Source { get => CurrentFrame.Source; }

        private bool frameChanged = false;
        public bool FrameChanged { get => frameChanged; }

        public enum AnimationState
        {
            Playing,
            Stopped,
            Paused
        }

        public void Update()
        {
            frameChanged = false;
            if (state == AnimationState.Stopped) return;
            if (state == AnimationState.Paused) return;

            if (frameElapsed >= CurrentFrame.Duration)
            {
                if (frameIndex < TotalFrames - 1)
                {
                    //Next frame
                    frameElapsed -= CurrentFrame.Duration;
                    frameIndex++;
                    frameChanged = true;
                }
                else
                {
                    if (bucle)
                    {
                        //Restart animation
                        frameElapsed -= CurrentFrame.Duration;
                        frameIndex = 0;
                        frameChanged = true;
                    }
                    else
                    {
                        //Finish animation
                        frameElapsed = 0f;
                        frameIndex = 0;
                        state = AnimationState.Stopped;
                    }
                }
            }
            frameElapsed += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
        }

        public void SetAnimation(AnimationData animation)
        {
            Stop();
            this.animation = animation;
            frameChanged = true;
        }

        public void Play()
        {
            frameIndex = 0;
            frameElapsed = 0f;
            state = AnimationState.Playing;
            frameChanged = true;
        }
        public void Resume()
        {
            if (state != AnimationState.Paused) return;
            state = AnimationState.Playing;
        }
        public void Pause()
        {
            if (state == AnimationState.Stopped) return;
            state = AnimationState.Paused;
        }
        public void Stop()
        {
            if (state == AnimationState.Stopped) return;
            state = AnimationState.Stopped;
        }

        public void SetFrame(int index)
        {
            frameIndex = index;
            frameElapsed = 0f;
            frameChanged = true;
        }
        public void SetAtTime(TimeSpan totalTime)
        {
            float totalTimeMili = (float)totalTime.TotalMilliseconds;
            int currentFrame = 0;
            while (true)
            {
                totalTimeMili -= animation.Frames[currentFrame].Duration;
                if (totalTimeMili >= 0f)
                {
                    currentFrame++;
                    if (currentFrame > TotalFrames - 1)
                    {
                        if (bucle) currentFrame = 0;
                        else
                        {
                            currentFrame = TotalFrames - 1;
                            break;
                        }
                    }
                }
                else break;
            }
            SetFrame(currentFrame);
        }
        public AnimationFrame GetFrameData(int index) => animation.Frames[index];
    }
}
