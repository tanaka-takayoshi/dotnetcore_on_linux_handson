# Part2 Entity Framework Core 1.0 の基本的な使い方

Part2では、実際にデータベースファイルからデータを取得します。用意されてあるファイルを利用することも可能ですが、EF Migrationでのデータベースファイルの生成も説明します。

データベースファイルの配置場所として、App_Dataディレクトリを作成します。公開しないファイルであるためwwwroot内である必要はなく((むしろ公開するのはよくない))、ディレクトリ名は任意です。また、Modelsディレクトリを作成し、Pubs.csファイルを作成さいます。

```bash
$ mkdir App_Data
$ mkdir Models
$ touch Models/Pubs.cs
```

Pubs.csはこのファイルの様に記述します。

[Pubs.cs](https://github.com/tanaka-takayoshi/dotnetcore_on_linux_handson/blob/master/aspnetcore/AspNetCorehandson/Models/Pubs.cs)

TBD: EFに関する説明

さて、データベースファイルですが、サンプルを利用する場合はこちら（TBD）からダウンロードして、App_Dataディレクトリに配置してください。
一から作成する場合は、EFのMigrationツールを利用します。最初のコマンドの最後の引数はマイグレーション名で任意の名前でよいです。
なお、これで作成されたデータベースは空ですので、適当にレコードを入力してください（SQL用意するかも、TBD）。

```bash
$ dotnet ef migrations add InitialSetup
$ dotnet ef database update
```

ここまでできたら確認のため、Startup.csファイル内のConfigureメソッドにある``app.Run``を書き換えてDBにアクセスできるか実行してみましょう。

```cs
app.Run(async (context) =>
  {
    using (PubsEntities pubs = new PubsEntities())
    {
      var query = pubs.Authors.Where(a => a.State == "CA");
      await context.Response.WriteAsync(query.Count().ToString());
    }
  });
```

サンプルファイルを利用した場合は15と表示されるはずです。
