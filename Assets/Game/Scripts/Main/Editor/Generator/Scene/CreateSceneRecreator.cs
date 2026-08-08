// 创建时间：2026-08-07
// 修改时间：2026-08-07
// 审核时间：

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

namespace Game.Scripts.Main.Editor.Generator.Scene
{
    /// <summary>
    /// 创角场景 (Create.unity) 自动化生成器。
    /// 包含下大雨与狂风粒子特效及气象氛围控制。
    /// </summary>
    public class CreateSceneRecreator : EditorWindow
    {
        [MenuItem("Generator/Scene/Recreate Create Scene")]
        public static void Recreate()
        {
            // 1. 创建新场景
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 2. 创建主摄像机
            GameObject cameraGo = new GameObject("Main Camera");
            Camera camera = cameraGo.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.06f, 0.08f, 0.12f, 1f); // 暗夜暴风雨色调
            camera.orthographic = true;
            camera.orthographicSize = 10;
            cameraGo.transform.position = new Vector3(0, 0, -10);
            cameraGo.transform.rotation = Quaternion.identity;

            // 3. 创建背景UI画布(Canvas)
            GameObject canvasGo = new GameObject("BackgroundCanvas");
            Canvas canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 90;

            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();

            // 4. 创建背景UI图片(Image)
            GameObject imageGo = new GameObject("BackgroundImage");
            imageGo.transform.SetParent(canvasGo.transform, false);
            Image bgImage = imageGo.AddComponent<Image>();

            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Background/Menu/menu_background.png");
            if (bgSprite != null)
            {
                bgImage.sprite = bgSprite;
                bgImage.color = new Color(0.6f, 0.65f, 0.75f, 1f); // 雨夜沉浸冷色调
            }
            else
            {
                Debug.LogWarning("menu_background.png not found at Assets/Game/Textures/Background/Menu/menu_background.png!");
            }

            RectTransform rt = bgImage.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;

            // 5. 创建下大雨粒子效果 (HeavyRainParticles)
            GameObject rainGo = new GameObject("HeavyRainParticles");
            rainGo.transform.position = new Vector3(0, 12, 10);
            rainGo.transform.rotation = Quaternion.Euler(75, -25, 0);

            ParticleSystem rainPs = rainGo.AddComponent<ParticleSystem>();
            var rainMain = rainPs.main;
            rainMain.duration = 10;
            rainMain.loop = true;
            rainMain.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 0.9f);
            rainMain.startSpeed = new ParticleSystem.MinMaxCurve(28f, 38f);
            rainMain.startSize = new ParticleSystem.MinMaxCurve(0.15f, 0.4f);
            rainMain.startColor = new Color(0.8f, 0.9f, 1.0f, 0.75f);
            rainMain.gravityModifier = 1.2f;
            rainMain.simulationSpace = ParticleSystemSimulationSpace.World;
            rainMain.maxParticles = 800;

            var rainEmission = rainPs.emission;
            rainEmission.rateOverTime = 350f; // 大雨滂沱密度

            var rainShape = rainPs.shape;
            rainShape.shapeType = ParticleSystemShapeType.Box;
            rainShape.scale = new Vector3(45f, 1f, 1f);

            var rainColorOverLifetime = rainPs.colorOverLifetime;
            rainColorOverLifetime.enabled = true;
            Gradient rainGradient = new Gradient();
            rainGradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(new Color(0.85f, 0.92f, 1.0f), 0.0f),
                    new GradientColorKey(new Color(0.75f, 0.85f, 0.98f), 1.0f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(0.0f, 0.0f),
                    new GradientAlphaKey(0.8f, 0.1f),
                    new GradientAlphaKey(0.8f, 0.8f),
                    new GradientAlphaKey(0.0f, 1.0f)
                }
            );
            rainColorOverLifetime.color = rainGradient;

            var rainVelocity = rainPs.velocityOverLifetime;
            rainVelocity.enabled = true;
            rainVelocity.x = new ParticleSystem.MinMaxCurve(-15f, -25f);
            rainVelocity.y = new ParticleSystem.MinMaxCurve(-20f, -30f);
            rainVelocity.z = new ParticleSystem.MinMaxCurve(0f, 0f);

            ParticleSystemRenderer rainRenderer = rainGo.GetComponent<ParticleSystemRenderer>();
            if (rainRenderer != null)
            {
                rainRenderer.renderMode = ParticleSystemRenderMode.Stretch;
                rainRenderer.lengthScale = 4.0f;
                rainRenderer.velocityScale = 0.05f;

                Material rainMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Game/Materials/Effects/Scene/Menu/part_snowflake_mat.mat");
                if (rainMat != null)
                {
                    rainRenderer.material = rainMat;
                }
            }

            // 6. 创建狂风吹拂粒子效果 (GaleWindParticles)
            GameObject windGo = new GameObject("GaleWindParticles");
            windGo.transform.position = new Vector3(18, 0, 10);

            ParticleSystem windPs = windGo.AddComponent<ParticleSystem>();
            var windMain = windPs.main;
            windMain.duration = 8;
            windMain.loop = true;
            windMain.startLifetime = new ParticleSystem.MinMaxCurve(1.8f, 3.2f);
            windMain.startSpeed = new ParticleSystem.MinMaxCurve(12f, 22f);
            windMain.startSize = new ParticleSystem.MinMaxCurve(3.0f, 7.0f);
            windMain.startColor = new Color(0.75f, 0.85f, 0.95f, 0.15f);
            windMain.simulationSpace = ParticleSystemSimulationSpace.World;
            windMain.maxParticles = 150;

            var windEmission = windPs.emission;
            windEmission.rateOverTime = 25f;

            var windShape = windPs.shape;
            windShape.shapeType = ParticleSystemShapeType.Box;
            windShape.scale = new Vector3(1f, 20f, 1f);

            var windColorOverLifetime = windPs.colorOverLifetime;
            windColorOverLifetime.enabled = true;
            Gradient windGradient = new Gradient();
            windGradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.white, 0.0f),
                    new GradientColorKey(Color.white, 1.0f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(0.0f, 0.0f),
                    new GradientAlphaKey(0.18f, 0.3f),
                    new GradientAlphaKey(0.18f, 0.7f),
                    new GradientAlphaKey(0.0f, 1.0f)
                }
            );
            windColorOverLifetime.color = windGradient;

            var windVelocity = windPs.velocityOverLifetime;
            windVelocity.enabled = true;
            windVelocity.x = new ParticleSystem.MinMaxCurve(-20f, -32f);
            windVelocity.y = new ParticleSystem.MinMaxCurve(0f, 0f);
            windVelocity.z = new ParticleSystem.MinMaxCurve(0f, 0f);

            var windNoise = windPs.noise;
            windNoise.enabled = true;
            windNoise.strength = 1.2f;
            windNoise.frequency = 0.6f;

            ParticleSystemRenderer windRenderer = windGo.GetComponent<ParticleSystemRenderer>();
            if (windRenderer != null)
            {
                Material windMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Game/Materials/Effects/Scene/Menu/part_snowflake_mat.mat");
                if (windMat != null)
                {
                    windRenderer.material = windMat;
                }
            }

            // 7. 创建雨滴溅起水花效果 (RainSplashParticles)
            GameObject splashGo = new GameObject("RainSplashParticles");
            splashGo.transform.position = new Vector3(0, -9, 10);

            ParticleSystem splashPs = splashGo.AddComponent<ParticleSystem>();
            var splashMain = splashPs.main;
            splashMain.duration = 5;
            splashMain.loop = true;
            splashMain.startLifetime = new ParticleSystem.MinMaxCurve(0.2f, 0.5f);
            splashMain.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
            splashMain.startSize = new ParticleSystem.MinMaxCurve(0.2f, 0.6f);
            splashMain.startColor = new Color(0.85f, 0.92f, 1.0f, 0.4f);
            splashMain.gravityModifier = 0.5f;
            splashMain.simulationSpace = ParticleSystemSimulationSpace.World;
            splashMain.maxParticles = 300;

            var splashEmission = splashPs.emission;
            splashEmission.rateOverTime = 120f;

            var splashShape = splashPs.shape;
            splashShape.shapeType = ParticleSystemShapeType.Box;
            splashShape.scale = new Vector3(40f, 0.5f, 1f);

            ParticleSystemRenderer splashRenderer = splashGo.GetComponent<ParticleSystemRenderer>();
            if (splashRenderer != null)
            {
                Material splashMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Game/Materials/Effects/Scene/Menu/part_snowflake_mat.mat");
                if (splashMat != null)
                {
                    splashRenderer.material = splashMat;
                }
            }

            // 8. 挂载场景控制器脚本并绑定私有字段
            var createCtrl = canvasGo.AddComponent<Game.Scripts.Hotfix.HotfixBusiness.Scene.Create.CreateSceneController>();
            SetPrivateField(createCtrl, "heavyRainParticles", rainPs);
            SetPrivateField(createCtrl, "galeWindParticles", windPs);
            SetPrivateField(createCtrl, "rainSplashParticles", splashPs);
            SetPrivateField(createCtrl, "windSound", 100002);
            SetPrivateField(createCtrl, "rainSound", 100003);

            // 9. 保存场景
            EditorSceneManager.SaveScene(newScene, "Assets/Game/Scenes/Create.unity");
            Debug.Log("Successfully recreated Create.unity scene with heavy rain and gale wind effects!");
        }

        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            if (obj == null) return;
            var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }
            else
            {
                Debug.LogWarning($"Field '{fieldName}' not found on '{obj.GetType().Name}'.");
            }
        }
    }
}
