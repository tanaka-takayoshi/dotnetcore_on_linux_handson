# Part5  ASP.NET Web API を使った SPA 型 Web アプリ開発

WebAPIを使ったSPA型のリスト表示を作ります。コントローラー、Viewを作成します。

```
$ yo aspnet:MvcController Sample01Controller
$ mkdir Views/Sample01
$ yo aspnet:MvcView Sample01/ShowAllAuthors
$ yo aspnet:MvcView Sample01/ShowAuthorsByState
```

Sample01ControllerにはまずViewを返すメソッドを実装します。

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCorehandson.ApiModels;
using AspNetCorehandson.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCorehandson.Controllers
{
    public class Sample01Controller : Controller
    {
        // GET: /<controller>/ShowAllAuthors
        [HttpGet]
        public IActionResult ShowAllAuthors()
        {
            return View();
        }

        // GET: /<controller>/ShowAuthorsByState
        [HttpGet]
        public IActionResult ShowAuthorsByState()
        {
            return View();
        }
    }
}
```

ShowAllAuthors.cshtmlと ShowAuthorsByState.cshtmlはひとまずタイトルだけ記述します。デバッグ実行して、このページにアクセスできることを確認します。

```html
@{ ViewData["Title"] = "全著者データの一覧"; }

<h4>全著者データの一覧</h4>
```

```html
@{ ViewData["Title"] = "州による著者データの検索"; }

<h4>州による著者データの検索</h4>
```

ControllerにWebAPIのメソッドを追加します。ASP.NET Core MVC にはWebAPIも含まれており、同じControllerの中に混在させることもできます。

```cs
[HttpGet]
public IList<Author> GetAllAuthors()
{
    using (var pubs = new PubsEntities())
    {
        return pubs.Authors.ToArray();
    }
}
```

これで、 http://localhost:xxxx/Sample01/GetAllAuthors にアクセスするとJSONが表示されるはずです。なお、ASP.NET Core MVC のRTMより、デフォルトでJSONのフィールド名が小文字始まりになっています。こちらのハンズオンではこのまま進め、JavaScript側でも小文字始まりとして扱うことにします。

ここで、Authorは全てのプロパティを返すのではなく、一覧表示用に一部のプロパティだけ返すようにしてみましょう。ViewModels/AuthorOverview.csを作成します。

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCorehandson.ViewModels
{
    public class AuthorOverview
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public bool Contract { get; set; }
    }
}
```

さきほど実装したGetAllAuthorsメソッドを次のように書き直します。

```cs
[HttpGet]
public IList<AuthorOverview> GetAllAuthors()
{
    using (var pubs = new PubsEntities())
    {
        return pubs.Authors
            .Select(a => new AuthorOverview
            {
                AuthorId = a.AuthorId,
                AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                Phone = a.Phone,
                State = a.State,
                Contract = a.Contract
            })
            .ToArray();
    }
}
```

ここからクライアントサイドの実装に入ります。
ShowAllAuthors.cshtml をまず次のように修正します。

```html

@{ ViewData["Title"] = "全著者データの一覧"; }

<h4>全著者データの一覧</h4>

@section Scripts {
    <script type="text/javascript">
        $(function () {
            $.getJSON("/Sample01/GetAllAuthors", function (result) {
                console.debug(result);
            });
        });
    </script>
}
```

動作確認できたら、knockout.jsを利用して表示することにします。まず、knockout.jsをインポートするためのShared Viewを /Views/Shared/_ImportsLibraryKnockout.cshtml に定義します。

```html
<script src="https://ajax.aspnetcdn.com/ajax/knockout/knockout-3.3.0.js"></script>
```

ShowAllAuthors.cshtml はこうなります、

```html
@{ ViewData["Title"] = "全著者データの一覧"; }

@section Libraries {
    @Html.Partial("_ImportsLibraryKnockout")
}

<h4>全著者データの一覧</h4>

<div class="table-responsive">
    <table class="table table-condensed table-striped table-hover">
        <thead>
            <tr>
                <th>著者ID</th>
                <th>著者名</th>
                <th>電話番号</th>
                <th>州</th>
                <th>契約有無</th>
            </tr>
        </thead>
        <tbody data-bind="foreach: authors">
            <tr>
                <td data-bind="text: authorId"></td>
                <td data-bind="text: authorName"></td>
                <td data-bind="text: phone"></td>
                <td data-bind="text: state"></td>
                <td>
                    <input type="checkbox" disabled data-bind="checked: contract" />&nbsp;
                    <text data-bind="text: (contract ? '契約あり' : '契約なし')"></text>
                </td>
            </tr>
        </tbody>
    </table>
</div>

@section Scripts {
    <script type="text/javascript">
        $(function () {
            var viewModel = {
                authors: ko.observableArray()
            };
            ko.applyBindings(viewModel);

            $.getJSON("/Sample01/GetAllAuthors", function (result) {
                viewModel.authors(result);
            });
        });
    </script>
}
```

次に州によるフィルタリングを実装します。州の一覧を返すAPIと、指定された州に属する著者の一覧を取得するAPIをSample01Controllerに追加します。

```cs
[HttpGet]
public IList<string> GetStates()
{
    using (var pubs = new PubsEntities())
    {
        var query = pubs.Authors.Select(a => a.State).Distinct();
        return query.ToArray();
    }
}

[HttpGet]
public IList<AuthorOverview> GetAuthorsByState(string state)
{
    if (Regex.IsMatch(state, "^[A-Z]{2}$") == false) throw new ArgumentOutOfRangeException(nameof(state));

    using (var pubs = new PubsEntities())
    {
        var query = pubs.Authors.Where(a => a.State == state)
                    .Select(a => new AuthorOverview()
                    {
                        AuthorId = a.AuthorId,
                        AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                        Phone = a.Phone,
                        State = a.State,
                        Contract = a.Contract
                    });
        return query.ToArray();
    }
}
```

AJAX関連の設定を入れる場合、_Layout.cshtml に記述するとよいだろう。ここでは、HTTP GETでのキャッシュの無効化と集約例外ハンドラを設定している。

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>@ViewData["Title"]</title>
    <meta name="viewport" content="width=device-width, intial-scale=1.0" />

    <script src="//code.jquery.com/jquery-2.1.4.min.js"></script>
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet">

    <script type="text/javascript">
    $(function () {
        $.ajaxSetup({
            cache: false,
            error: function (xhr, status, err) { alert("通信エラーが発生しました。"); } // 集約通信例外ハンドラ
        });

        window.onerror = function (message, url, lineNumber) {
            console.log(message);
            var msg = "処理中にエラーが発生しました。" + message;
            alert(msg);
            return true;
        };
    });
    </script>
    
    @RenderSection("Libraries", required: false)
    //略
```

ShowAuthorsByState.cshtml を次のように実装します。

```html
@{ ViewData["Title"] = "州による著者データの検索"; }

@section Libraries {
    @Html.Partial("_ImportsLibraryKnockout")
}

<h4>州による著者データの検索</h4>

<div>
    <select id="ddlStates" data-bind="options: states"></select>
    <button id="btnShowAuthors">データ表示</button>
</div>

<hr />

<div class="table-responsive">
    <table id="tblAuthors" class="table table-condensed table-striped table-hover">
        <thead>
            <tr>
                <th>著者ID</th>
                <th>著者名</th>
                <th>電話番号</th>
                <th>州</th>
                <th>契約有無</th>
            </tr>
        </thead>
        <tbody data-bind="foreach: authors">
            <tr>
                <td data-bind="text: authorId"></td>
                <td data-bind="text: authorName"></td>
                <td data-bind="text: phone"></td>
                <td data-bind="text: state"></td>
                <td>
                    <input type="checkbox" disabled data-bind="checked: contract" />&nbsp;
                    <text data-bind="text: (contract ? '契約あり' : '契約なし')"></text>
                </td>
            </tr>
        </tbody>
    </table>
</div>

@section Scripts {
    <script type="text/javascript">
        $(function () {
            $("#tblAuthors").hide(); // css('display', 'none') と同じ

            // 後から値を入れたい場合には、ko.observable() と ko.observableArray() を割り当てておく
            var viewModel = {
                states: ko.observableArray(),
                authors: ko.observableArray()
            };
            ko.applyBindings(viewModel);

            // サーバから州一覧を取り寄せてバインド
            $.getJSON("/Sample01/GetStates", function (result) {
                viewModel.states(result);
            });

            $("#btnShowAuthors").click(function () {
                // クエリ文字列を引数に渡すには、第二パラメータにオブジェクトを渡す
                $.getJSON("/Sample01/GetAuthorsByState", { state: $("#ddlStates").val() }, function (result) {
                    viewModel.authors(result); // データを observableArray に流し込み
                    $("#tblAuthors").show(); // css('display', 'block') と同じ
                });
            });
        });
    </script>
}
```