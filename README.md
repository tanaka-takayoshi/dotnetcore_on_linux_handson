# .NET Core on Linux Handson


### 0. 環境構築

- dotnet tools
http://dot.net/

- Visual Studio Code
- C# extension
- npm
- yo, gulp, bower etc

### 1. .NET Core

- プロジェクトを新規作成してVisual Studio Codeで編集、デバッグ
- デバッガでBreak Pointの設定、変数のウォッチ
- dotnet buildで実行可能なバイナリを生成

#### プロジェクト間の参照、テスト

- ライブラリプロジェクトとアプリケーションプロジェクトを作成し、ライブラリを参照
- テストプロジェクトを作成し、テストを実行

#### NuGet

- NuGet ServerをNuGet.configに追加
- ライブラリプロジェクトを自前NuGetサーバーに追加

#### デプロイメント

- portable 
- selfcontained
- to windows

### 2. ASP.NET Core

#### [Hello World, ASP.NET Core 1.0!](/aspnetcore)

[こちらのブログ](https://blogs.msdn.microsoft.com/nakama/2016/07/07/aspnetcore10/) をVisual Studio Code on Linuxに焼き直したもの。

#### Database

- EFの導入
- CRUD操作
- DB Migration

(Option)
- Dapper


#### デプロイメント

- Linux VMへ
- Docker on Linux VM へ
- Docker on Azure Container Servies へ
- Azure WebAppsへ
- OpenShiftへ
