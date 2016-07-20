# Part7 ASP.NET Web API による SPA 型データ更新アプリ

Sample03ControllerとViewを作成します。

```shell
$ yo aspnet:MvcController Sample02Controller
$ mkdir Views/Sample02
$ yo aspnet:MvcView Sample02/ListAuthors
$ yo aspnet:MvcView Sample02/EditAuthors
```

著者一覧ページを作成します。

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCorehandson.Models;
using AspNetCorehandson.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCorehandson.Controllers
{
    public class Sample03Controller : Controller
    {
        [HttpGet]
        public ActionResult ListAuthors()
        {
            return View();
        }

        [HttpGet]
        public IList<AuthorOverview> GetAuthors()
        {
            using (var pubs = new PubsEntities())
            {
                var query = pubs.Authors.Select(a => new AuthorOverview
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
    }
}
```

Views/Sample03/ListAuthors.cshtml

```html
@{
    ViewBag.Title = "編集対象の著者選択";
}
@section Libraries {
    @Html.Partial("_ImportsLibraryKnockout")
}

<h4>編集対象となる著者を選択してください。</h4>

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
                <td><a data-bind="text: authorId, attr: { href: 'EditAuthor/' + authorId }"></a></td>
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

<hr />

<p>
    <a href="/">業務メニューに戻る</a>
</p>

@section Scripts {
    <script type="text/javascript">
        $(function () {
            // ブラウザキャッシュ無効化 (これを入れておかないと、Edit ページから戻ってきたときにリストが更新されない)
            $.ajaxSetup({
                cache: false
            });

            $("#tblAuthors").hide(); // css('display', 'none') と同じ

            // 後から値を入れたい場合には、ko.observable() と ko.observableArray() を割り当てておく
            var viewModel = {
                states: ko.observableArray(),
                authors: ko.observableArray()
            };
            ko.applyBindings(viewModel);

            $.getJSON("/Sample03/GetAuthors", null, function (result) {
                viewModel.authors(result); // データを observableArray に流し込み
                $("#tblAuthors").show(); // css('display', 'block') と同じ

                console.dir(result); // 全体表示
            });

        });
    </script>
}
```

次に編集ページを作成します。AJAXで投げる編集リクエストオブジェクトを作成します。

ViewModels/UpdateAuthorRequest.cs

```cs
using System.ComponentModel.DataAnnotations;

namespace AspNetCorehandson.ViewModels
{
    public class UpdateAuthorRequest
    {
        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{2}-[0-9]{4}$")]
        public string AuthorId { get; set; }

        [Required(ErrorMessage = "著者名（名）は必須入力項目です。")]
        [RegularExpression(@"^[\u0020-\u007e]{1,20}$", ErrorMessage = "著者名（名）は半角 20 文字以内で指定してください。")]
        public string AuthorFirstName { get; set; }

        [Required(ErrorMessage = "著者名（姓）は必須入力項目です。")]
        [RegularExpression(@"^[\u0020-\u007e]{1,40}$", ErrorMessage = "著者名（姓）は半角 40 文字以内で指定してください。")]
        public string AuthorLastName { get; set; }

        [Required(ErrorMessage = "電話番号は必須入力項目です。")]
        [RegularExpression(@"^\d{3} \d{3}-\d{4}$", ErrorMessage = "電話番号は 012 345-6789 のような形式で指定してください。")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "州は必須入力項目です。")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "州は半角大文字 2 文字で指定してください。")]
        public string State { get; set; }
    }
}
```

Sample03Controllerに編集ページを送信するGETアクションメソッド、画面に表示する著者データおよび州一覧データを取得するアクションメソッド、編集リクエストをPOSTで受け付けるアクションメソッドを作成します。

```cs
[HttpGet]
public IList<string> GetAllStates()
{
    using (var pubs = new PubsEntities())
    {
        return pubs.Authors.Select(a => a.State).Distinct().ToArray();
    }
}

[HttpGet]
public ActionResult EditAuthor(string id)
{
    if (Regex.IsMatch(id, @"^[0-9]{3}-[0-9]{2}-[0-9]{4}$") == false) throw new ArgumentOutOfRangeException(nameof(id));
    ViewData["AuthorId"] = id;
    return View(new UpdateAuthorRequest());
}


[HttpGet]
public Author GetAuthorByAuthorId(string authorId)
{
    if (Regex.IsMatch(authorId, @"^[0-9]{3}-[0-9]{2}-[0-9]{4}$") == false) throw new ArgumentOutOfRangeException(nameof(authorId));

    // 当該著者 ID のデータを読み取る
    using (var pubs = new PubsEntities())
    {
        return pubs.Authors.FirstOrDefault(a => a.AuthorId == authorId);
    }
}

[HttpPost]
public void UpdateAuthor(UpdateAuthorRequest request)
{
    if (ModelState.IsValid == false) throw new ArgumentException();

    // データベースに登録する
    using (var pubs = new PubsEntities())
    {
        var target = pubs.Authors.FirstOrDefault(a => a.AuthorId == request.AuthorId);
        target.AuthorFirstName = request.AuthorFirstName;
        target.AuthorLastName = request.AuthorLastName;
        target.Phone = request.Phone;
        target.State = request.State;
        pubs.SaveChanges();
    }
}
```

Viewを作成します。

```html
@{
    ViewBag.Title = "編集対象の著者選択";
}
@section Libraries {
    @Html.Partial("_ImportsLibraryKnockout")
}

<h4>編集対象となる著者を選択してください。</h4>

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
                <td><a data-bind="text: authorId, attr: { href: 'EditAuthor/' + authorId }"></a></td>
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

<hr />

<p>
    <a href="/">業務メニューに戻る</a>
</p>

@section Scripts {
    <script type="text/javascript">
        $(function () {
            // ブラウザキャッシュ無効化 (これを入れておかないと、Edit ページから戻ってきたときにリストが更新されない)
            $.ajaxSetup({
                cache: false
            });

            $("#tblAuthors").hide(); // css('display', 'none') と同じ

            // 後から値を入れたい場合には、ko.observable() と ko.observableArray() を割り当てておく
            var viewModel = {
                states: ko.observableArray(),
                authors: ko.observableArray()
            };
            ko.applyBindings(viewModel);

            $.getJSON("/Sample03/GetAuthors", null, function (result) {
                viewModel.authors(result); // データを observableArray に流し込み
                $("#tblAuthors").show(); // css('display', 'block') と同じ

                console.dir(result); // 全体表示
            });

        });
    </script>
}
```
