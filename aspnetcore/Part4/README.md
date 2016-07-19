
### Part4


bootstrapを利用して、Index.cshtmlを書き直します。jsやcssはインターネット経由で取得していますが、ページの表示に利用しているプロトコルと同じプロコトルでアクセスするように、httpやhttpsのプロトコルを省略しています。

```html
@model AspNetCorehandson.ViewModels.AuthorListViewModel

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>de:code 2016</title>
    <meta name="viewport" content="width=device-width, intial-scale=1.0" />

    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet">
    <script src="//code.jquery.com/jquery-2.1.4.min.js"></script>
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>

    <style type="text/css">
        @@media only screen and (min-width : 768px) {
            html {
                position: relative;
                height: 100%;
            }
            body {
                height: 100%;
                padding-top: 50px;
                padding-bottom: 50px;
            }
            .contentWrapper {
                overflow: auto;
                height: 100%;
            }
            .contentBody {
                padding-top: 10px;
                padding-bottom: 10px;
                min-height: 100%;
            }
            .footer {
                position: fixed;
                bottom: 0;
                margin-bottom: 0;
                width: 100%;
                height: 50px;
                background-color: #f5f5f5;
            }
            .navbar-custom-responsive {
                position: fixed;
                width:100%;
                top:0px;
                border-width: 0 0 1px 0;
            }
        }
        @@media only screen and (min-width : 0px) {
            html {
            }
            body {
            }
            .contentWrapper {
            }
            .contentBody {
                padding-top: 10px;
                padding-bottom: 10px;
            }
            .footer {
                bottom: 0;
                margin-bottom: 0;
                width: 100%;
                height: 50px;
                background-color: #f5f5f5;
            }
            .navbar-custom-responsive {
                margin-bottom: 0;
            }
        }
    </style>

    <style type="text/css">
        div.row ul {
            padding-left: 30px;
        }
    </style>


</head>
<body>
    <nav class="navbar navbar-static-top navbar-default navbar-custom-responsive" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <a class="navbar-brand" href="/"><span class="glyphicon glyphicon-leaf"></span>&nbsp;&nbsp;de:code 2016</a>
                <button type="button" class="navbar-toggle" data-target="#navbar" data-toggle="collapse">
                    <span class="sr-only">ナビゲーションの表示</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
            </div>
            <div class="navbar-collapse collapse" id="navbar">
                <ul class="nav navbar-nav navbar-right">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                            各種サンプル&nbsp;<span class="caret"></span>
                        </a>
                        <ul class="dropdown-menu">
                            <li><a href="/Sample01/FilterByStateWithSort">従来型の Web アプリ</a></li>
                            <li><a href="/Sample02/FilterByStateWithSort">SPA 型の Web アプリ</a></li>
                            <li><a href="/Sample03/FilterByState">jQuery + Bootstrap + knockout.js</a></li>
                            <li><a href="/Sample04/ListAuthors">非 SPA ベースでの実装</a></li>
                            <li><a href="/Sample05/ListAuthors">SPA ベースでの実装</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <div class="contentWrapper">
        <div class="container contentBody">
            <table class="table table-condensed table-striped table-hover">
                <thead>
                    <tr>
                        <th>著者ID</th>
                        <th>著者名</th>
                        <th>電話番号</th>
                        <th>契約有無</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var a in Model.Authors)
                    {
                        <tr>
                            <td>@a.AuthorId</td>
                            <td>@a.AuthorFirstName @a.AuthorLastName</td>
                            <td>@a.Phone</td>
                            <td>@(a.Contract ? "契約あり" : "契約なし")</td>
                        </tr>
                    }
                </tbody>
            </table>

        </div>
    </div>

    <footer class="footer">
        <div class="text-right" style="padding-top: 13px; padding-right: 20px; ">&copy; 2016 Microsoft Corporation. All rights reserved.</div>
    </footer>
</body>
</html>
```

ここまで作ったら一度デバッグ実行します。
次のように表示されているはずです。
![](https://raw.githubusercontent.com/tanaka-takayoshi/dotnetcore_on_linux_handson/master/aspnetcore/images/part4/list.png)

次にViewの構造化を行います。Views/Sharedフォルダを作成し、その中に_Layout.cshtmlファイルを作成します。

```sh
$ mkdir Views/Shared
$ yo aspnet:MvcView Shared/_Layout
```

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>@ViewData["Title"]</title>
    <meta name="viewport" content="width=device-width, intial-scale=1.0" />

    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet">
    <script src="//code.jquery.com/jquery-2.1.4.min.js"></script>
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>

    @RenderSection("Libraries", required: false)

    <style type="text/css">
        @@media only screen and (min-width : 768px) {
            html {
                position: relative;
                height: 100%;
            }

            body {
                height: 100%;
                padding-top: 50px;
                padding-bottom: 50px;
            }

            .contentWrapper {
                overflow: auto;
                height: 100%;
            }

            .contentBody {
                padding-top: 10px;
                padding-bottom: 10px;
                min-height: 100%;
            }

            .footer {
                position: fixed;
                bottom: 0;
                margin-bottom: 0;
                width: 100%;
                height: 50px;
                background-color: #f5f5f5;
            }

            .navbar-custom-responsive {
                position: fixed;
                width: 100%;
                top: 0px;
                border-width: 0 0 1px 0;
            }
        }

        @@media only screen and (min-width : 0px) {
            html {
            }

            body {
            }

            .contentWrapper {
            }

            .contentBody {
                padding-top: 10px;
                padding-bottom: 10px;
            }

            .footer {
                bottom: 0;
                margin-bottom: 0;
                width: 100%;
                height: 50px;
                background-color: #f5f5f5;
            }

            .navbar-custom-responsive {
                margin-bottom: 0;
            }
        }
    </style>

    @RenderSection("Styles", required: false)

</head>
<body>
    <nav class="navbar navbar-static-top navbar-default navbar-custom-responsive" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <a class="navbar-brand" href="/"><span class="glyphicon glyphicon-leaf"></span>&nbsp;&nbsp;de:code 2016</a>
                <button type="button" class="navbar-toggle" data-target="#navbar" data-toggle="collapse">
                    <span class="sr-only">ナビゲーションの表示</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
            </div>
            <div class="navbar-collapse collapse" id="navbar">
                <ul class="nav navbar-nav navbar-right">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                            各種サンプル&nbsp;<span class="caret"></span>
                        </a>
                        <ul class="dropdown-menu">
                            <li><a href="/Sample01/FilterByStateWithSort">従来型の Web アプリ</a></li>
                            <li><a href="/Sample02/FilterByStateWithSort">SPA 型の Web アプリ</a></li>
                            <li><a href="/Sample03/FilterByState">jQuery + Bootstrap + knockout.js</a></li>
                            <li><a href="/Sample04/ListAuthors">非 SPA ベースでの実装</a></li>
                            <li><a href="/Sample05/ListAuthors">SPA ベースでの実装</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <div class="contentWrapper">
        <div class="container contentBody">

            @RenderBody()

        </div>
    </div>

    <footer class="footer">
        <div class="text-right" style="padding-top: 13px; padding-right: 20px; ">&copy; 2016 Microsoft Corporation. All rights reserved.</div>
    </footer>

    @RenderSection("Scripts", required: false)

</body>
</html>
```

また、デフォルトでこのLayoutを利用する場合は、View/_ViewStart.cshtmlを作成します。

```html
@{
    Layout = "_Layout";
}
```

Index.html はこのようになります。

```html
@model AspNetCorehandson.ViewModels.AuthorListViewModel
@{ 
    ViewData["Title"] = "著者データ一覧";
}

<table class="table table-condensed table-striped table-hover">
    <thead>
        <tr>
            <th>著者ID</th>
            <th>著者名</th>
            <th>電話番号</th>
            <th>契約有無</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var a in Model.Authors)
        {
            <tr>
                <td>@a.AuthorId</td>
                <td>@a.AuthorFirstName @a.AuthorLastName</td>
                <td>@a.Phone</td>
                <td>@(a.Contract ? "契約あり" : "契約なし")</td>
            </tr>
        }
    </tbody>
</table>
```

デバッグ実行して表示が変わらないことを確認しましょう。