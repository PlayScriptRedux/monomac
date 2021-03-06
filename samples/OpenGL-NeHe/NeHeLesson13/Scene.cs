//
// This code was created by Jeff Molofee '99
//
// If you've found this code useful, please let me know.
//
// Visit me at www.demonews.com/hosted/nehe
//
//=====================================================================
// Converted to C# and MonoMac by Kenneth J. Pouncey
// http://www.cocoa-mono.org
//
// Copyright (c) 2011 Kenneth J. Pouncey
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
using System;
using System.Drawing;

using MonoMac.Foundation;
using MonoMac.AppKit;

using MonoMac.OpenGL;

namespace NeHeLesson13
{
	public class Scene : NSObject
	{
		// 1st counter used to move text and for coloring
		float cnt1;
		// 2nd counter used to move text and for coloring
		float cnt2;
		
		// Bitmap Font
		BitmapFont bmFont;
		
		public Scene () : base()
		{
			bmFont = new BitmapFont("Monaco", 24);
		}

		// Resize And Initialize The GL Window 
		//      - See also the method in the MyOpenGLView Constructor about the NSView.GlobalFrameDidChangeNotification
		public void ResizeGLScene (RectangleF bounds)
		{
			// Reset The Current Viewport
			GL.Viewport (0, 0, (int)bounds.Size.Width, (int)bounds.Size.Height);
			// Select The Projection Matrix
			GL.MatrixMode (MatrixMode.Projection);
			// Reset The Projection Matrix
			GL.LoadIdentity ();

			// Set perspective here - Calculate The Aspect Ratio Of The Window
			Perspective (45, bounds.Size.Width / bounds.Size.Height, 0.1, 100);

			// Select The Modelview Matrix
			GL.MatrixMode (MatrixMode.Modelview);
			// Reset The Modelview Matrix
			GL.LoadIdentity ();
		}

		// This creates a symmetric frustum.
		// It converts to 6 params (l, r, b, t, n, f) for glFrustum()
		// from given 4 params (fovy, aspect, near, far)
		public static void Perspective (double fovY, double aspectRatio, double front, double back)
		{
			const
			double DEG2RAD = Math.PI / 180 ; 

			// tangent of half fovY
			double tangent = Math.Tan (fovY / 2 * DEG2RAD);

			// half height of near plane
			double height = front * tangent;

			// half width of near plane
			double width = height * aspectRatio;

			// params: left, right, bottom, top, near, far
			GL.Frustum (-width, width, -height, height, front, back);
		}

		// This method renders our scene and where all of your drawing code will go.
		// The main thing to note is that we've factored the drawing code out of the NSView subclass so that
		// the full-screen and non-fullscreen views share the same states for rendering 
		public bool DrawGLScene ()
		{
			// Clear The Screen And The Depth Buffer
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			// Reset The Current Modelview Matrix
			GL.LoadIdentity ();

			// Move one unit into the screen
			GL.Translate (0, 0, -1);

			// Pulsing Colors Based On Text Position
			GL.Color3 (1.0f * (float)Math.Cos (cnt1), 1.0f * (float)Math.Sin (cnt2), 1.0f - 0.5f * (float)Math.Cos (cnt1 + cnt2));			

			// Position the text on the screen
			GL.RasterPos2 (-0.45f + 0.05f * (float)Math.Cos (cnt1), 0.35f * (float)Math.Sin (cnt2));
			
			// Print GL Text To The Screen
			bmFont.RenderText (String.Format ("Active OpenGL Text With NeHe - {0:#######.00}", cnt1));

			// Increase first counter
			cnt1 += 0.051f;
			// Increase second counter
			cnt2 += 0.005f;

			return true;
		}

	}
}

