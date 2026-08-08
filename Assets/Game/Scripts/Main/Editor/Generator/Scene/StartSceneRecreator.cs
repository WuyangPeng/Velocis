using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Main.Editor.Generator.Scene
{
    public class StartSceneRecreator : EditorWindow
    {
        [MenuItem("Generator/Scene/Recreate Start Scene")]
        public static void Recreate()
        {
            // 1. 创建新场景
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            
            // 2. 创建主摄像机
            GameObject cameraGo = new GameObject("Main Camera");
            Camera camera = cameraGo.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.orthographic = true;
            camera.orthographicSize = 10;
            cameraGo.transform.position = new Vector3(0, 0, -10);
            cameraGo.transform.rotation = Quaternion.identity;
            
            // 3. 创建背景UI画布(Canvas)
            GameObject canvasGo = new GameObject("BackgroundCanvas");
            Canvas canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 90; // 背景面片距离较远
            
            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();
            
            // 4. 创建背景UI图片(Image)
            GameObject imageGo = new GameObject("BackgroundImage");
            imageGo.transform.SetParent(canvasGo.transform, false);
            Image bgImage = imageGo.AddComponent<Image>();
            
            // 加载背景图精灵
            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Background/Menu/menu_background.png");
            if (bgSprite != null)
            {
                bgImage.sprite = bgSprite;
            }
            else
            {
                Debug.LogWarning("menu_background.png not found at Assets/Game/Textures/Background/Menu/menu_background.png!");
            }
            
            // 设置RectTransform为全屏拉伸
            RectTransform rt = bgImage.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;

            // 4b. 创建闪电UI图片(Image)
            GameObject lightningGo = new GameObject("LightningImage");
            lightningGo.transform.SetParent(canvasGo.transform, false);
            Image lightningImage = lightningGo.AddComponent<Image>();
            
            // 加载闪电精灵
            Sprite lightningSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Effects/Scene/Menu/part_lightning.png");
            if (lightningSprite != null)
            {
                lightningImage.sprite = lightningSprite;
            }
            else
            {
                Debug.LogWarning("part_lightning.png not found!");
            }
            
            // 材质球(使用粒子叠加Shader实现发光效果)
            Material lightningMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Game/Materials/Effects/Scene/Menu/part_lightning_mat.mat");
            if (lightningMat != null)
            {
                lightningImage.material = lightningMat;
            }
            
            // 设置闪电大小与位置
            RectTransform lightningRt = lightningImage.GetComponent<RectTransform>();
            lightningRt.anchorMin = new Vector2(0.5f, 0.5f);
            lightningRt.anchorMax = new Vector2(0.5f, 0.5f);
            lightningRt.sizeDelta = new Vector2(512, 512);
            lightningRt.anchoredPosition = Vector2.zero;
            
            // 挂载闪电控制器脚本并设置音效 ID
            var lightningCtrl = canvasGo.AddComponent<Game.Scripts.Hotfix.HotfixBusiness.Scene.Start.LightningController>();
            var field = typeof(Game.Scripts.Hotfix.HotfixBusiness.Scene.Start.LightningController).GetField("lightningImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(lightningCtrl, lightningImage);
            }
            var soundField = typeof(Game.Scripts.Hotfix.HotfixBusiness.Scene.Start.LightningController).GetField("sound", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (soundField != null)
            {
                soundField.SetValue(lightningCtrl, 100000);
            }

            // 5. 创建下雪粒子效果
            GameObject particlesGo = new GameObject("HeavySnowParticles");
            particlesGo.transform.position = new Vector3(0, 11, 10); // 放置在屏幕视口顶部 Z=10
            particlesGo.transform.rotation = Quaternion.Euler(90, 0, 0); // 向下发射
            
            ParticleSystem ps = particlesGo.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.duration = 10;
            main.loop = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(4f, 7f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(2.5f, 4.5f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.25f, 0.9f); // 放大初始大小以展现雪花细节
            main.startColor = new Color(1f, 1f, 1f, 0.9f); // 纯白雪花
            main.gravityModifier = 0.05f; // 模拟重力缓缓下落
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = 120;
            
            var emission = ps.emission;
            emission.rateOverTime = 15f; // 控制落雪密度，更加清爽雅致
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(40f, 1f, 1f); // 粒子发射盒范围横向拉宽铺满屏幕
            
            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(Color.white, 0.0f), 
                    new GradientColorKey(Color.white, 1.0f) 
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.0f, 0.0f), 
                    new GradientAlphaKey(0.9f, 0.1f), 
                    new GradientAlphaKey(0.9f, 0.8f),
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = gradient;
            
            var velocityOverLifetime = ps.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(-1.2f, -0.4f); // 模拟风向（从右向左吹）
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f, 0f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f, 0f);
            
            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = 0.35f;
            noise.frequency = 0.5f;
            
            // 关联雪花粒子材质球防止纹理丢失
            ParticleSystemRenderer psRenderer = particlesGo.GetComponent<ParticleSystemRenderer>();
            if (psRenderer != null)
            {
                Material snowMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Game/Materials/Effects/Scene/Menu/part_snowflake_mat.mat");
                if (snowMat != null)
                {
                    psRenderer.material = snowMat;
                }
            }
            
            // 保存场景
            EditorSceneManager.SaveScene(newScene, "Assets/Game/Scenes/Start.unity");
            Debug.Log("Successfully recreated Start.unity scene with dynamic background UI stretch and beautiful heavy snowfall!");
        }
    }
}
