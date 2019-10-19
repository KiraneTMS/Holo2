using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Urho;
using Urho.Actions;
using Urho.SharpReality;
using Urho.Shapes;
using Urho.Resources;
using Urho.Gui;

namespace HoloUrhoSharp2
{
    internal class Program
    {
        [MTAThread]
        static void Main()
        {
            var appViewSource = new UrhoAppViewSource<HelloWorldApplication>(new ApplicationOptions("Data"));
            appViewSource.UrhoAppViewCreated += OnViewCreated;
            CoreApplication.Run(appViewSource);
        }

        static void OnViewCreated(UrhoAppView view)
        {
            view.WindowIsSet += View_WindowIsSet;
        }

        static void View_WindowIsSet(Windows.UI.Core.CoreWindow coreWindow)
        {
            // you can subscribe to CoreWindow events here
        }
    }

    public class HelloWorldApplication : StereoApplication
    {

        //Node earthNode;
        Node robotNode;

        public HelloWorldApplication(ApplicationOptions opts) : base(opts) { }

        protected override async void Start()
        {
            // Create a basic scene, see StereoApplication
            base.Start();

            // Enable input
            EnableGestureManipulation = true;
            EnableGestureTapped = true;

            /*
            // Create a node for the Earth
            earthNode = Scene.CreateChild();
            earthNode.Position = new Vector3(0, 0, 1.5f); //1.5m away
            earthNode.SetScale(0.3f); //D=30cm
            */

            // create a node for robot
            robotNode = Scene.CreateChild();
            
            // Manipulation
            robotNode.Position = new Vector3(0, 0, 1.5f);
            robotNode.SetScale(0.005f);
            //robotNode.Rotation = new Quaternion(0, 90, -90);
            //robotNode.Translate(new Vector3(0, 0, 1), TransformSpace.World);

            // Scene has a lot of pre-configured components, such as Cameras (eyes), Lights, etc.
            DirectionalLight.Brightness = 1f;
            DirectionalLight.Node.SetDirection(new Vector3(-1, 0, 0.5f));

            /*
            //Sphere is just a StaticModel component with Sphere.mdl as a Model.
            var earth = earthNode.CreateComponent<Sphere>();
            earth.Material = Material.FromImage("Textures/Earth.jpg");
            */

            var robot = robotNode.CreateComponent<StaticModel>();
            robot.Model = ResourceCache.GetModel(@"Data\Battery\isulated_casing.mdl");
            //robot.SetMaterial(Material.SkyboxFromImage("Textures/orange.jpg"));
            /*
            var duaNode = robotNode.CreateChild();
            duaNode.Position = new Vector3(1.2f, 0, 0);

            var dua = duaNode.CreateComponent<StaticModel>();
            dua.Model = ResourceCache.GetModel(@"Data\Battery\isulated_casing_2.mdl");
            */
            /*
            var moonNode = earthNode.CreateChild();
            moonNode.SetScale(0.27f); //27% of the Earth's size
            moonNode.Position = new Vector3(1.2f, 0, 0);

            // Same as Sphere component:
            var moon = moonNode.CreateComponent<StaticModel>();
            moon.Model = CoreAssets.Models.Sphere;

            moon.Material = Material.FromImage("Textures/Moon.jpg");

            // Run a few actions to spin the Earth, the Moon and the clouds.
            earthNode.RunActions(new RepeatForever(new RotateBy(duration: 1f, deltaAngleX: 0, deltaAngleY: -4, deltaAngleZ: 0)));
            await TextToSpeech("Hello world from UrhoSharp!");


            // More advanced samples can be found here:
            // https://github.com/xamarin/urho-samples/tree/master/HoloLens
            */

            await TextToSpeech("Welcome, Foreheader, The Forehead Lovers");

            Sprite crosshair = new Sprite(Context);
            crosshair.Texture = ResourceCache.GetTexture2D(@"Data/Textures/pokemon.png");
            crosshair.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            crosshair.Position = new IntVector2(0, 0);
            crosshair.SetSize(25, 25);
            UI.Root.AddChild(crosshair);
            
        }

        // For HL optical stabilization (optional)
        //public override Vector3 FocusWorldPoint => earthNode.WorldPosition;
        public override Vector3 FocusWorldPoint => robotNode.WorldPosition;

        //Handle input:
        /*
        Vector3 earthPosBeforeManipulations;
        public override void OnGestureManipulationStarted() => earthPosBeforeManipulations = earthNode.Position;
        public override void OnGestureManipulationUpdated(Vector3 relativeHandPosition) =>
            earthNode.Position = relativeHandPosition + earthPosBeforeManipulations;

        public override void OnGestureTapped() { }
        public override void OnGestureDoubleTapped() { }
        */


        Vector3 robotPosBeforeManipulations;
        public override void OnGestureManipulationStarted() => robotPosBeforeManipulations = robotNode.Position;
        public override void OnGestureManipulationUpdated(Vector3 relativeHandPosition) =>
            robotNode.Position = relativeHandPosition + robotPosBeforeManipulations;



        public override void OnGestureTapped() {

            robotNode.Enabled = false;

            Text text = new Text(Context);
            text.Value = "Anda Telah Meng-Disable Robot";
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Top;
            text.Position = new IntVector2(0, 100);
            text.SetColor(Color.White);
            text.SetFont(CoreAssets.Fonts.AnonymousPro, 30);
            UI.Root.AddChild(text);



            Ray cameraRay = RightCamera.GetScreenRay(0.5f, 0.5f);

            var result = Scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry, 0x70000000);

            robotNode = Scene.CreateChild();
            robotNode.Name = "Destroyer";

            if (result != null && result.Value.Node.Name == "Destroyer")
            {

                text.Value = "Anda Telah Meng-Klik Di Robot";
                

            }
            
        }
        public override void OnGestureDoubleTapped() {
            robotNode.Enabled = true;
        }
    }
}