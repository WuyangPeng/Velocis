# Velocis

这个项目是一个基于Unity引擎的游戏项目。

## 项目结构

项目的核心代码位于 `Assets/Game/Scripts` 目录下，主要分为两个部分：

*   **Main**: 主工程，包含游戏的入口和核心框架代码。
    *   `Main/Runtime`: 主工程的运行时代码。
    *   `Main/Editor`: 主工程的编辑器代码。
*   **Hotfix**: 热更新工程，包含游戏的业务逻辑。
    *   `HotfixBusiness`: 业务逻辑。
    *   `HotfixCommon`: 通用代码。
    *   `HotfixFramework`: 热更新框架。
    *   `HotfixMain`: 热更新入口。

## 核心框架

项目基于 GameFramework 框架进行开发，这是一个强大的Unity游戏开发框架，提供了实体、UI、流程、数据表、本地化等多种模块。

## 配置管理

项目使用 Luban 来管理和生成配置文件。Luban是一个强大的跨平台配置解决方案，可以将Excel等格式的配置文件转换为程序可读的格式。

*   Luban的配置文件位于 `Tools/Luban` 目录下。
*   生成的代码和数据位于 `Assets/Game/Scripts/Main/Runtime/Luban` 和 `Assets/Game/DataTables/bytes` 目录下。

## 游戏启动流程

游戏的启动流程由 `ProcedureLaunch.cs` 文件定义，主要步骤如下：

1.  **初始化语言设置**: 根据用户设置或系统默认设置语言。
2.  **初始化资源变体**: 根据当前语言设置对应的资源变体。
3.  **初始化声音设置**: 根据用户设置初始化声音选项。
4.  **加载默认字典**: 加载默认的本地化字典。
5.  **切换到Splash流程**: 进入闪屏界面。

## 主要依赖

项目使用了一系列Unity官方和第三方的包，定义在 `Packages/manifest.json` 文件中，主要包括：

*   `com.unity.render-pipelines.universal`: Universal Render Pipeline (URP)。
*   `com.unity.nuget.newtonsoft-json`: Newtonsoft.Json for Unity。
*   `com.unity.textmeshpro`: TextMesh Pro。
*   `com.jiangyin.gameframework`: GameFramework 框架。

## 总结

Velocis 是一个结构清晰、基于 GameFramework 和 Luban 的 Unity 游戏项目。它采用了主工程+热更新的开发模式，可以快速迭代和更新游戏内容。
