### Part1 

まずはじめに、空のASP.NET Core Web Applicationプロジェクトを作成します。``dotnet new -t web`` の場合、MVCのフルセットが作成されるため、今回はYeamanを利用します。``yo aspnet``と入力するとウィザード形式で選択することができます。

```bash
$ yo aspnet
```

上下矢印で移動できますが、初期値の「Empty Web Application」を選択します。

![](https://raw.githubusercontent.com/tanaka-takayoshi/dotnetcore_on_linux_handson/master/aspnetcore/images/part1/01Yemon.png)

プロジェクト名を入力します。Yeomanはカレントディレクトリにプロジェクト名と同じディレクトリを作成し、そのディレクトリ内にファイルを作成します。現状、プロジェクト名とディレクトリ名および既定の名前空間が同じ値になります。
![](https://raw.githubusercontent.com/tanaka-takayoshi/dotnetcore_on_linux_handson/master/aspnetcore/images/part1/02Yemon.png)

作成されたディレクトリでVisual Studio Codeを開きます。Insiderではない場合、``code``コマンドです。
```bash
$ code AspNetCorehandson
```

Insiderの場合は``code-insiders``コマンドになります。
```bash
$ code-insiders AspNetCorehandson
```

開くと、アセットのダウンロードを求められるのでYesを選択します。``.vscode``ディレクトリが作成されます。
![](https://raw.githubusercontent.com/tanaka-takayoshi/dotnetcore_on_linux_handson/master/aspnetcore/images/part1/03VSCode.png)

``F5``キーを押してデバッグ起動します。左側のアイコン一覧から虫アイコンを選び、緑矢印アイコンでも起動できます。自動でブラウザが立ち上がりメッセージが表示されるはずです。起動しない場合は、http://localhost:5000 を開いてください。
![](https://raw.githubusercontent.com/tanaka-takayoshi/dotnetcore_on_linux_handson/master/aspnetcore/images/part1/04Debug.png)

Visual Studio Codeに表示されている赤四角アイコンを押すなどしてデバッグを修了します。``project.json``ファイルを開いて、下記の様に修正します。なお、本家ではSQLServerを利用していますが、localdb(mdfファイル)がLinux上で利用できないため、SQLiteを指定しています。

dependenciesに以下のライブラリを追加。
```json
  "dependencies": {
  //略
    "Microsoft.AspNetCore.Mvc": "1.0.0",
    "Microsoft.AspNetCore.StaticFiles": "1.0.0",
    "Microsoft.AspNetCore.Mvc.TagHelpers": "1.0.0",
    "Microsoft.EntityFrameworkCore": "1.0.0",
    "Microsoft.EntityFrameworkCore.Sqlite": "1.0.0",
    "Microsoft.EntityFrameworkCore.Design": {
      "version": "1.0.0-preview2-final",
      "type": "build"
    }
  }
```

toolsに以下のライブラリを追加。
```json
  "tools": {
    "Microsoft.AspNetCore.Server.IISIntegration.Tools": "1.0.0-preview2-final",
    "Microsoft.EntityFrameworkCore.Tools": "1.0.0-preview2-final"
  }
```

追加したら、restoreしてエラーなく完了することを確認します。

```bash
$ dotnet restore
```
