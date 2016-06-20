# .NET Core on Linux Handson


### 0. 環境構築

- dotnet tools
http://dot.net/

- Visual Studio Code
- C# extension

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

TBD

### 2. ASP.NET Core

#### HelloWeb

- Hello WorldプロジェクトをWebプロジェクトへ

#### Hello MVC with Yemon

- Yemonの導入
- テンプレートWebプロジェクトの生成
- Webアプリのデバッグ

#### Database

- EFの導入
- CRUD操作
- DB Migration

(Option)
- Dapper

#### 既成アプリからの操作

TBD

#### デプロイメント

- Linux VMへ
- Docker on Linux VM へ
- Docker on Azure Container Servies へ
- Azure WebAppsへ
- OpenShiftへ
