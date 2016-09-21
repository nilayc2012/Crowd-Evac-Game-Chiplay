using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*! \The Heatmap class is responsible for creating the "heat" and textures.
 *http://paraboxstudios.com/heatmap/docs/class_heatmap.html
 *  Contains all methods necessary for creating a Heatmap.
 */
public class Heatmap : ScriptableObject
{

    public static GameObject projectionPlane;   //!< A static reference to the plane which is used to display a heatmap.

    public static int RED_THRESHOLD = 255;      //!< Red threshold.
                                                ///	Minimum alpha a point must have to be red.
    public static int GREEN_THRESHOLD = 100;    //!< Green threshold.
                                                ///	Minimum alpha a point must have to be green.
    public static int BLUE_THRESHOLD = -333;     //!< Blue threshold.	
                                                 ///	Minimum alpha a point must have to be Blue.
    public static int MINIMUM_THRESHOLD = -333;  //!< Minimum threshold.	
                                                 ///	Minimum alpha a point must have to be rendered at all.
                                                 /// 
    //Inputs:   speedAndPos | the dictionary that stores all the points and their different color values
    //          cam         | the main camera
    //          plane       | the heatplane that the texture is going to be put on
    //Outputs:  Texture2D   | the texture that the heatmap is put on
    //Functionality: creates a texture for the heatmap to be stored on using all the points and their color values 
    public static Texture2D CreateHeatmap(Dictionary<Vector2, float> speedAndPos, Camera cam, GameObject plane)
    {
        //find camera
        if (cam == null)
        {
            if (Camera.main == null)
            {
                Debug.LogWarning("No camera found.  Returning an empty texture.");
                return new Texture2D(0, 0);
            }
            else
                cam = Camera.main;
        }

        // Create new texture
        Texture2D map = new Texture2D((int)(plane.transform.localScale.x * 30 * 10), (int)(plane.transform.localScale.z * 30 * 10), TextureFormat.ARGB32, false);

        // Set texture to alpha-fied state
        map.SetPixels(Heatmap.ColorArray(new Color(1f, 1f, 1f, 0f), map.width * map.height), 0);

        //Going through each point, set the alpha value to the color value in speedAndPos
        foreach (KeyValuePair<Vector2, float> keyval in speedAndPos)
        {
            //this checks if the color value is 0, in which case the color should be different. This represents the starting position of the agent
            //this is currently turned off in Tracker
            if (keyval.Value == 0)
            {
                map.SetPixel((int)keyval.Key.x, (int)keyval.Key.y, Color.cyan);
            }
            else
            {
                map.SetPixel((int)keyval.Key.x, (int)keyval.Key.y, new Color(1f, 1f, 1f, keyval.Value));
            }
        }

        //finish the map by applying all changes and setting the rest of the pixels
        map.Apply();
        map.SetPixels(Colorize(map.GetPixels(0)), 0);
        map.Apply();

        return map;
    }


    /*!	<summary>
	*	Creates a gameObject in front of the camera, and applies the supplied texture.
	*	</summary>
	*	<remarks>
	*	Works best with an orthographic camera.  It builds the mesh using camera dimensions translated into world space.  Use this in conjunction with CreateHeatmap() to create a heatmap and capture a screenshot with the heatmap texture overlaying the world.
	*	</remarks>
	*	<param name="map">The heatmap image.</param>
	*/
    public static void CreateRenderPlane(Texture2D map)
    {
        CreateRenderPlane(map, null);
    }

    /*!	<summary>
	*	Creates a gameObject in front of the camera, and applies the supplied texture.
	*	</summary>
	*	<remarks>
	*	Works best with an orthographic camera.  It builds the mesh using camera dimensions translated into world space.  Use this in conjunction with CreateHeatmap() to create a heatmap and capture a screenshot with the heatmap texture overlaying the world.
	*	</remarks>
	*	<param name="map">The heatmap image.</param>
	*	<param name="cam">The camera to render from.  If passed a null value, CreateRenderPlane() attempts to use Camera.main.</param>
	*/
    public static void CreateRenderPlane(Texture2D map, Camera cam)
    {
        if (cam == null)
        {
            if (Camera.main == null)
            {
                Debug.LogWarning("No camera found.  Plane not created.");
                return;
            }
            else
                cam = Camera.main;
        }

        Material mat = (Material)Resources.Load("UnlitMaterial");
        mat.mainTexture = map;
    }

    /*!	<summary>
	*	Creates and saves a screenshot.
	*	</summary>
	*	<remarks>
	*	Call this to take a screenshot with the current camera.  Will not overwrite images if path already exists.
	*	</remarks>
	*	<param name="map">The path to save screenshot image to.  Path is relative to Unity project. Ex: "Assets/MyScreenshot.png"</param>
	*	<returns>
	*	Returns a string containing the actual path image was saved to.  This may be different than passed string if the path previously existed.
	*	</returns>
	*/
    public static string Screenshot(string path)
    {
        return Heatmap.Screenshot(path, (Camera)null);
    }

    /*!	<summary>
	*	Creates and saves a screenshot.
	*	</summary>
	*	<remarks>
	*	Call this to take a screenshot with the current camera.  Will not overwrite images if path already exists.
	*	</remarks>
	*	<param name="path">The path to save screenshot image to.  Path is relative to Unity project. Ex: "Assets/MyScreenshot.png"</param>
	*	<param name="cam">The camera to render from.  If passed a null value, CreateRenderPlane() attempts to use Camera.main.</param>
	*	<returns>
	*	Returns a string containing the actual path image was saved to.  This may be different than passed string if the path previously existed.
	*	</returns>
	*/
    public static string Screenshot(string path, Camera cam)
    {
        if (cam == null)
        {
            if (Camera.main == null)
                return "Error!  No camera found.";
            else
                cam = Camera.main;
        }

        foreach (Camera c in Camera.allCameras)
            c.enabled = false;

        cam.enabled = true;
        int i = 0;

        while (System.IO.File.Exists(path))
            path = path.Replace(".png", ++i + ".png");

        Application.CaptureScreenshot(path, 4);



        return path;
    }


    public static Color[] ColorArray(Color col, int arraySize)
    {
        Color[] colArr = new Color[arraySize];
        for (int i = 0; i < colArr.Length; i++)
        {
            colArr[i] = col;
        }
        return colArr;
    }

    /*
	 *	!!!
	 *	The Colorize() function is a modified version of the Colorize() method found
	 *	in the Mapex library.  This is the license associated with it.  This license
	 *	does not apply to the rest of the codebase included in this project, as it is
	 *	covered by the Unity Asset Store EULA.
	 *
	 * Copyright (C) 2011 by Vinicius Carvalho (vinnie@androidnatic.com)
	 *
	 * Permission is hereby granted, free of charge, to any person obtaining a copy
	 * of this software and associated documentation files (the "Software"), to deal
	 * in the Software without restriction, including without limitation the rights
	 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	 * copies of the Software, and to permit persons to whom the Software is
	 * furnished to do so, subject to the following conditions:
	 * 
	 * The above copyright notice and this permission notice shall be included in
	 * all copies or substantial portions of the Software.
	 * 
	 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	 * THE SOFTWARE.
	 */
    public static Color[] Colorize(Color[] pixels)
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] == Color.cyan)
            {
                continue;
            }
            float r = 0, g = 0, b = 0, tmp = 0;
            pixels[i] *= 255f;

            float alpha = pixels[i].a;

            if (alpha == 0)
            {
                continue;
            }

            if (alpha <= 255 && alpha >= RED_THRESHOLD)
            {
                tmp = 255 - alpha;
                r = 255 - tmp;
                g = tmp * 12f;
            }
            else if (alpha <= (RED_THRESHOLD - 1) && alpha >= GREEN_THRESHOLD)
            {
                tmp = (RED_THRESHOLD - 1) - alpha;
                r = 255 - (tmp * 8f);
                g = 255;
            }
            else if (alpha <= (GREEN_THRESHOLD - 1) && alpha >= BLUE_THRESHOLD)
            {
                tmp = (GREEN_THRESHOLD - 1) - alpha;
                g = 255;
                b = tmp * 5;
            }
            else if (alpha <= (BLUE_THRESHOLD - 1) && alpha >= MINIMUM_THRESHOLD)
            {
                tmp = (BLUE_THRESHOLD - 1) - alpha;
                g = 255 - (tmp * 5f);
                b = 255;
            }
            else
                b = 255;
            pixels[i] = new Color(r, g, b, alpha / 2f);
            pixels[i] = NormalizeColor(pixels[i]);
        }

        return pixels;
    }

    public static Color NormalizeColor(Color col)
    {
        return new Color(col.r / 255f, col.g / 255f, col.b / 255f, col.a / 255f);
    }


}
