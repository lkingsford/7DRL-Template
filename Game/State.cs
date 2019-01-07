using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// UI State of the application
    /// </summary>
    class State
    {

        public State()
        {
            if (gui == null)
            {
                inputListener = new InputListenerComponent(GameApp);
                inputManager = new MonoGame.Extended.NuclexGui.GuiInputService(inputListener);
                gui = new GuiManager(GameApp.Services, inputManager);
                gui.Initialize();
            }
            gui.Screen = new GuiScreen();
        }

        /// <summary>
        /// Application wide sprite batch to use for drawing operations
        /// </summary>
        static public SpriteBatch AppSpriteBatch;

        /// <summary>
        /// Application wide content manager
        /// </summary>
        static public ContentManager AppContentManager;

        /// <summary>
        /// Application wide graphics device
        /// </summary>
        static public GraphicsDevice AppGraphicsDevice;

        /// <summary>
        /// Application wide stack of states
        /// </summary>
        static public List<State> StateStack;

        /// <summary>
        /// Width of the screen, per Monogame.Game
        /// </summary>
        static public int ScreenWidth;

        /// <summary>
        /// Height of the screen, per Monogame.Game
        /// </summary>
        static public int ScreenHeight;

        static protected InputListenerComponent inputListener;
        static protected GuiInputService inputManager;
        static protected GuiManager gui;

        /// <summary>
        /// The Monogame Game
        /// </summary>
        static public Microsoft.Xna.Framework.Game GameApp;

        /// <summary>
        /// Run logic for this state - including input
        /// </summary>
        /// <param name="GameTime">Snapshot of timing</param>
        public virtual void Update(GameTime GameTime)
        {

        }

        /// <summary>
        /// Draw this state
        /// </summary>
        /// <param name="GameTime">Snapshot of timing</param>
        public virtual void Draw(GameTime GameTime)
        {

        }
    }
}
