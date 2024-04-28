using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

namespace Celeste.Mod.DzhakeHelper.Entities
{
    [CustomEntity("DzhakeHelper/CustomDecal")]
    public class CustomDecal : Decal
    {

        // -_-
        private static float camScale = 6f;
        private static Vector2 entityOffset = new(12f, 12f);

        public enum SpriteBank
        {
            Game, Gui, Portraits, Misc
        };
        public SpriteBank PathRoot;

        public string ImagePath;

        public Sprite Sprite;

        public bool Animated;

        public string Flag;

        public bool UpdateSpriteOnlyIfFlag;

        public bool InversedFlag;

        public string AnimationName;
        public string fullPath;

        public float Delay;

        public Vector2 shakeOffset;

        public bool HD;

        public CustomDecal(EntityData data, Vector2 offset) : this(data.Position, offset, data.Attr("imagePath"), data.Int("depth"), data.HexColorWithAlpha("color"), new Vector2(data.Float("scaleX"),
            data.Float("scaleY")), data.Float("rotation"), data.Bool("animated"), data.Attr("flag"), data.Bool("updateSpriteOnlyIfFlag"), data.Bool("inversedFlag"), data.Attr("animationName"),
            data.Float("delay"), data.Bool("attached"), data.Bool("hiRes"), data.Enum("pathRoot",SpriteBank.Game))
        {}

        public CustomDecal(Vector2 position,Vector2 offset, string imagePath, int depth, Color color, Vector2 scale, float rotation, bool animated, string flag, bool updateSpriteOnlyIfFlag, bool inversedFlag,
            string animationName, float delay, bool attached, bool hd, SpriteBank PathRoot) : base(imagePath, position + offset, scale, depth)
        {
            base.Depth = depth;

            this.PathRoot = PathRoot;

            HD = hd;
            if (HD)
            {
                Tag = TagsExt.SubHUD;
            }

            Flag = flag;
            UpdateSpriteOnlyIfFlag = updateSpriteOnlyIfFlag;
            InversedFlag = inversedFlag;

            ImagePath = imagePath;
            Animated = animated;
            AnimationName = animationName;
            Delay = delay;

            Color = color;
            Scale = scale;
            Rotation = rotation;

            fullPath = Animated ? ImagePath + AnimationName : ImagePath;

            //-_-
            if (!HD /*i don't want to debug random decal registry + hd combination :v*/ && DecalRegistry.RegisteredDecals.TryGetValue(fullPath, out DecalRegistry.DecalInfo info))
            {
                foreach (KeyValuePair<string, XmlAttributeCollection> property in info.CustomProperties.OrderByDescending(p => p.Equals("scale")))
                {
                    if (DecalRegistry.PropertyHandlers.ContainsKey(property.Key))
                    {
                        try
                        {
                            PropertyHandlers[property.Key].Invoke(this, property.Value);
                        }
                        catch (Exception e)
                        {
                            LevelEnter.ErrorMessage = Dialog.Get("postcard_decalregerror").Replace("((property))", property.Key).Replace("((decal))", fullPath);
                            Logger.Log(LogLevel.Warn, "Decal Registry", $"Failed to apply property '{property.Key}' to {fullPath}");
                        }

                    }
                    else
                    {
                        Logger.Log(LogLevel.Warn, "Decal Registry", $"Unknown property {property.Key} in decal {fullPath}");
                    }
                }
            }

            if (attached)
            {
                Add(new StaticMover
                {
                    OnShake = OnShake,
                    OnEnable = OnEnable,
                    OnDisable = OnDisable
                });
            }

            UpdateSprite();
        }

        public override void Update()
        {
            if (Animated && Sprite != null && UpdateSpriteOnlyIfFlag)
            {
                if (FlagIsTrue())
                {
                    Sprite.Rate = 1f;
                }
                else
                {
                    Sprite.Rate = 0f;
                }
            }
            base.Update();
        }

        public void UpdateSprite()
        {
            Atlas atlas = PathRoot switch
            {
                SpriteBank.Game => GFX.Game,
                SpriteBank.Gui => GFX.Gui,
                SpriteBank.Portraits => GFX.Portraits,
                SpriteBank.Misc => GFX.Misc,
                _ => GFX.Game,
            };

            if (Sprite != null)
            {
                Remove(Sprite);
            }
            if (true)//Animated)
            {
                Sprite = new Sprite(atlas,ImagePath);
                Add(Sprite);
                Sprite.Color = Color;
                Sprite.Scale = Scale;
                Sprite.Rotation = Rotation;
                Sprite.AddLoop("normal", AnimationName, Delay);
                Sprite.Play("normal");
            }
            else
            {
                //Image = atlas[ImagePath];
            }
        }

        public bool FlagIsTrue()
        {
            return Flag == "" || ((base.Scene as Level).Session.GetFlag(Flag) != InversedFlag);
        }


        private void OnShake(Vector2 amount)
        {
            shakeOffset += amount;
        }

        private void OnEnable()
        {
            Active = (Visible = (Collidable = true));
        }

        private void OnDisable()
        {
            Active = Visible = Collidable = false;
        }

        public static Dictionary<string, Action<CustomDecal, XmlAttributeCollection>> PropertyHandlers = new Dictionary<string, Action<CustomDecal, XmlAttributeCollection>>() {
            { "parallax", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                if (attrs["amount"] != null)
                    decal.MakeParallax(float.Parse(attrs["amount"].Value));
            }},
            { "scale", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                float scaleX = attrs["multiplyX"] != null ? float.Parse(attrs["multiplyX"].Value) : 1f;
                float scaleY = attrs["multiplyY"] != null ? float.Parse(attrs["multiplyY"].Value) : 1f;
                decal.Scale *= new Vector2(scaleX, scaleY);
            }},
            { "smoke", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                float offX = attrs["offsetX"] != null ? float.Parse(attrs["offsetX"].Value) : 0f;
                float offY = attrs["offsetY"] != null ? float.Parse(attrs["offsetY"].Value) : 0f;
                bool inbg = attrs["inbg"] != null ? bool.Parse(attrs["inbg"].Value) : false;

                Vector2 offset = decal.GetScaledOffset(offX, offY);

                decal.CreateSmoke(offset, inbg);
            }},
            { "depth", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                if (attrs["value"] != null)
                    decal.Depth = int.Parse(attrs["value"].Value);
            }},
            { "animationSpeed", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                if (attrs["value"] != null)
                    decal.AnimationSpeed = int.Parse(attrs["value"].Value);
            }},
            { "floaty", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                decal.MakeFloaty();
            }},
            { "sound", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                if (attrs["event"] != null)
                    decal.Add(new SoundSource(attrs["event"].Value));
            }},
            { "bloom", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                float offX = attrs["offsetX"] != null ? float.Parse(attrs["offsetX"].Value) : 0f;
                float offY = attrs["offsetY"] != null ? float.Parse(attrs["offsetY"].Value) : 0f;
                float alpha = attrs["alpha"] != null ? float.Parse(attrs["alpha"].Value) : 1f;
                float radius = attrs["radius"] != null ? float.Parse(attrs["radius"].Value) : 1f;

                Vector2 offset = decal.GetScaledOffset(offX, offY);
                radius = decal.GetScaledRadius(radius);

                decal.Add(new BloomPoint(offset, alpha, radius));
            }},
            { "coreSwap", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                decal.MakeFlagSwap("cold", attrs["hotPath"]?.Value, attrs["coldPath"]?.Value);
            }},
            { "mirror", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                string text = decal.Name.ToLower();
                if (text.StartsWith("decals/"))
                    text = text.Substring(7);
                bool keepOffsetsClose = attrs["keepOffsetsClose"] != null ? bool.Parse(attrs["keepOffsetsClose"].Value) : false;
                decal.MakeMirror(text, keepOffsetsClose);
            }},
            { "banner", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                float speed = attrs["speed"] != null ? float.Parse(attrs["speed"].Value) : 1f;
                float amplitude = attrs["amplitude"] != null ? float.Parse(attrs["amplitude"].Value) : 1f;
                int sliceSize = attrs["sliceSize"] != null ? int.Parse(attrs["sliceSize"].Value) : 1;
                float sliceSinIncrement = attrs["sliceSinIncrement"] != null ? float.Parse(attrs["sliceSinIncrement"].Value) : 1f;
                bool easeDown = attrs["easeDown"] != null ? bool.Parse(attrs["easeDown"].Value) : false;
                float offset = attrs["offset"] != null ? float.Parse(attrs["offset"].Value) : 0f;
                bool onlyIfWindy = attrs["onlyIfWindy"] != null ? bool.Parse(attrs["onlyIfWindy"].Value): false;

                amplitude *= decal.Scale.X;
                offset *= Math.Sign(decal.Scale.X) * Math.Abs(decal.Scale.Y);

                decal.MakeBanner(speed, amplitude, sliceSize, sliceSinIncrement, easeDown, offset, onlyIfWindy);
            }},
            { "solid", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                float x = attrs["x"] != null ? float.Parse(attrs["x"].Value) : 0f;
                float y = attrs["y"] != null ? float.Parse(attrs["y"].Value) : 0f;
                float width = attrs["width"] != null ? float.Parse(attrs["width"].Value) : 16f;
                float height = attrs["height"] != null ? float.Parse(attrs["height"].Value) : 16f;
                int index = attrs["index"] != null ? int.Parse(attrs["index"].Value) : SurfaceIndex.ResortRoof;
                bool blockWaterfalls = attrs["blockWaterfalls"] != null ? bool.Parse(attrs["blockWaterfalls"].Value) : true;
                bool safe = attrs["safe"] != null ? bool.Parse(attrs["safe"].Value) : true;

                decal.ScaleRectangle(ref x, ref y, ref width, ref height);

                decal.MakeSolid(x, y, width, height, index, blockWaterfalls, safe);
            }},
            { "staticMover", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                int x = attrs["x"] != null ? int.Parse(attrs["x"].Value) : 0;
                int y = attrs["y"] != null ? int.Parse(attrs["y"].Value) : 0;
                int width = attrs["width"] != null ? int.Parse(attrs["width"].Value) : 16;
                int height = attrs["height"] != null ? int.Parse(attrs["height"].Value) : 16;

                decal.ScaleRectangle(ref x, ref y, ref width, ref height);

                decal.MakeStaticMover(x, y, width, height);
            }},
            { "animation", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                int[] frames = Calc.ReadCSVIntWithTricks(attrs["frames"]?.Value ?? "0");

                decal.MakeAnimation(frames);
            }},
            { "scared", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                int hideRange = 32;
                int showRange = 48;
                if (attrs["range"] != null)
                    hideRange = showRange = int.Parse(attrs["range"].Value);
                if (attrs["hideRange"] != null)
                    hideRange = int.Parse(attrs["hideRange"].Value);
                if (attrs["showRange"] != null)
                    showRange = int.Parse(attrs["showRange"].Value);
                int[] idleFrames = Calc.ReadCSVIntWithTricks(attrs["idleFrames"]?.Value ?? "0");
                int[] hiddenFrames = Calc.ReadCSVIntWithTricks(attrs["hiddenFrames"]?.Value ?? "0");
                int[] hideFrames = Calc.ReadCSVIntWithTricks(attrs["hideFrames"]?.Value ?? "0");
                int[] showFrames = Calc.ReadCSVIntWithTricks(attrs["showFrames"]?.Value ?? "0");

                hideRange = (int) decal.GetScaledRadius(hideRange);
                showRange = (int) decal.GetScaledRadius(showRange);

                decal.MakeScaredAnimation(hideRange, showRange, idleFrames, hiddenFrames, showFrames, hideFrames);
            }},
            { "randomizeFrame", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                decal.RandomizeStartingFrame();
            }},
            { "light", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                float offX = attrs["offsetX"] != null ? float.Parse(attrs["offsetX"].Value) : 0f;
                float offY = attrs["offsetY"] != null ? float.Parse(attrs["offsetY"].Value) : 0f;
                Color color = attrs["color"] != null ? Calc.HexToColor(attrs["color"].Value) : Color.White;
                float alpha = attrs["alpha"] != null ? float.Parse(attrs["alpha"].Value) : 1f;
                int startFade = attrs["startFade"] != null ? int.Parse(attrs["startFade"].Value) : 16;
                int endFade = attrs["endFade"] != null ? int.Parse(attrs["endFade"].Value) : 24;

                Vector2 offset = decal.GetScaledOffset(offX, offY);
                startFade = (int) decal.GetScaledRadius(startFade);
                endFade = (int) decal.GetScaledRadius(endFade);

                decal.Add(new VertexLight(offset, color, alpha, startFade, endFade));
            }},
            { "lightOcclude", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                int x = attrs["x"] != null ? int.Parse(attrs["x"].Value) : 0;
                int y = attrs["y"] != null ? int.Parse(attrs["y"].Value) : 0;
                int width = attrs["width"] != null ? int.Parse(attrs["width"].Value) : 16;
                int height = attrs["height"] != null ? int.Parse(attrs["height"].Value) : 16;
                float alpha = attrs["alpha"] != null ? float.Parse(attrs["alpha"].Value) : 1f;

                decal.ScaleRectangle(ref x, ref y, ref width, ref height);

                decal.Add(new LightOcclude(new Rectangle(x, y, width, height), alpha));
            }},
            { "overlay", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                decal.MakeOverlay();
            }},
            { "flagSwap", delegate(CustomDecal decal, XmlAttributeCollection attrs) {
                if (attrs["flag"] != null)
                    decal.MakeFlagSwap(attrs["flag"].Value, attrs["offPath"]?.Value, attrs["onPath"]?.Value);
            }},
        };
    }
}
