# Part6 ASP.NET MVC Coreによるデータ更新アプリ

まず、Sample02ControllerとViewを追加します。

```
$ yo aspnet:MvcController Sample02Controller
$ mkdir Views/Sample02
$ yo aspnet:MvcView Sample02/ListAuthors
$ yo aspnet:MvcView Sample02/EditAuthors
```

著者一覧ページを作るためにViewModelを追加し、Contollerとビューを編集します。

ViewModels/AuthorOverviewListViewModel.cs

```cs
using AspNetCorehandson.Models;

namespace AspNetCorehandson.ViewModels
{
    public class AuthorOverviewListViewModel
    {
        public AuthorOverview[] Authors { get; set; }
    }
}
```

Sample02Controller

```cs
[HttpGet]
public ActionResult ListAuthors()
{
    using (var pubs = new PubsEntities())
    {
        var query = pubs.Authors
                    .Select(a => new AuthorOverview
                    {
                        AuthorId = a.AuthorId,
                        AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                        Phone = a.Phone,
                        State = a.State,
                        Contract = a.Contract
                    });
        var vm = new AuthorOverviewListViewModel
        {
            Authors = query.ToArray()
        };
        return View(vm);
    }
}
```

ListAuthors.cshtml

```html
@using AspNetCorehandson.ViewModels
@model AuthorOverviewListViewModel
@{
    ViewBag.Title = "編集対象の著者選択";
}

<h4>編集対象となる著者を選択してください。</h4>

@{
    if (Model.Authors != null)
    {
        var data = Model.Authors as IList<AuthorOverview>;
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
                <tbody>
                    @foreach (var a in data)
                    {
                        <tr>
                            <td><a href="/Sample02/EditAuthor/@a.AuthorId">@a.AuthorId</a></td>
                            <td>@a.AuthorName</td>
                            <td>@a.Phone</td>
                            <td>@a.State</td>
                            <td><input type="checkbox" disabled @(a.Contract ? "checked" : "") /></td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    }
}

<hr />

<p>
    <a href="/">業務メニューに戻る</a>
</p>
```

次に編集ページを作成します。編集ページのViewModelはValidationの属性を追加しておきます。

```cs
using System.ComponentModel.DataAnnotations;

namespace AspNetCorehandson.ViewModels
{
    public class AuthorEditViewModel
    {
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

Sample02Controller にHTTP GETのEditAuthorメソッドを追加します。これは編集画面を表示する際に呼ばれるアクションメソッドです。

```cs
[HttpGet]
public ActionResult EditAuthor(string id)
{
    // 当該著者 ID のデータを読み取る
    Author editAuthor = null;
    using (var pubs = new PubsEntities())
    {
        editAuthor = pubs.Authors.FirstOrDefault(a => a.AuthorId == id);
    }

    // View に引き渡すデータを準備する
    var vm = new AuthorEditViewModel()
    {
        AuthorId = editAuthor.AuthorId,
        AuthorFirstName = editAuthor.AuthorFirstName,
        AuthorLastName = editAuthor.AuthorLastName,
        Phone = editAuthor.Phone,
        State = editAuthor.State
    };

    // View にデータを引き渡すにあたり、入力データと周辺データを分けておく。
    // (ViewModel に周辺データを入れることで、ViewModel を完全にフォームモデルに一致させるように設計)
    using (var pubs = new PubsEntities())
    {
        var query = pubs.Authors.Select(a => a.State).Distinct();
        ViewData["AllStates"] = query.ToList();
    }

    return View(vm);
}
```

jQuery Validationを使うためのShared Viewを作成します。

Views/Shared/_ImportsLibraryValidation.cshtml

```html
<script src="//ajax.aspnetcdn.com/ajax/jquery.validate/1.14.0/jquery.validate.min.js"></script>
<script src="//ajax.aspnetcdn.com/ajax/jquery.validate/1.14.0/localization/messages_ja.js"></script>
<script src="//ajax.aspnetcdn.com/ajax/mvc/5.2.3/jquery.validate.unobtrusive.min.js"></script>
```

Views/Shared/_ImportsStyleValidation.cshtml

```html
<style type="text/css">
    @@media only screen and (min-width : 0px) and (max-width : 767px) {
    }


    @@media only screen and (min-width : 768px) and (max-width : 991px) {
        dl {
            width: 738px; /* 750-12 */
            margin: 6px;
        }


        dl dt {
            float: left;
        }


        dl dd {
            margin-left: 200px;
        }
    }


    @@media only screen and (min-width : 992px) and (max-width : 1199px) {
        dl {
            width: 958px; /* 970-12 */
            margin: 6px;
        }


        dl dt {
            float: left;
        }


        dl dd {
            margin-left: 200px;
        }
    }


    @@media only screen and (min-width : 1200px) {
        dl {
            width: 1158px; /* 1170-12 */
            margin: 6px;
        }


        dl dt {
            float: left;
        }


        dl dd {
            margin-left: 200px;
        }
    }


    /* エラーメッセージ用 */
    /* jQuery unobtrusive validation 用 */
    .field-validation-error {
        color: #ff0000;
    }


    .field-validation-valid {
        display: none;
    }


    .input-validation-error {
        border: 2px solid #ff0000;
        background-color: #ffeeee;
    }


    .validation-summary-errors {
        font-weight: bold;
        color: #ff0000;
    }


    .validation-summary-valid {
        display: none;
    }


    /* jQuery Validation 用 */
    .error { 
        color:red 
    }
    input.error, select.error, textarea.error {
        border: 2px solid red;
        background-color: #ffeeee;
    }


</style>
```

編集画面として、EditAuthor.cshtml を実装します。

```html

@model AspNetCorehandson.ViewModels.AuthorEditViewModel
@{
    ViewBag.Title = "著者データの編集";
}
@section Libraries {
    @Html.Partial("_ImportsLibraryValidation")
}

@section Styles {
    @Html.Partial("_ImportsStyleValidation")
}

<h4>著者データを修正してください。</h4>

@using (Html.BeginForm("EditAuthor", "Sample02", new { id = Model.AuthorId }))
{
    <dl>
        <dt>著者ID</dt>
        <dd>@Model.AuthorId</dd>
    </dl>
    <dl>
        <dt>著者名（名）</dt>
        <dd>@Html.TextBoxFor(m => m.AuthorFirstName, new { data_val_specialnamecheck = "指定された名前（名・姓の組み合わせ）は使えません。" }) @Html.ValidationMessageFor(m => m.AuthorFirstName, "*")</dd>
    </dl>
    <dl>
        <dt>著者姓（姓）</dt>
        <dd>@Html.TextBoxFor(m => m.AuthorLastName, new { data_val_specialnamecheck = "指定された名前（名・姓の組み合わせ）は使えません。" }) @Html.ValidationMessageFor(m => m.AuthorLastName, "*")</dd>
    </dl>
    <dl>
        <dt>電話番号</dt>
        <dd>@Html.TextBoxFor(m => m.Phone) @Html.ValidationMessageFor(m => m.Phone, "*")</dd>
    </dl>
    <dl>
        <dt>州</dt>
        <dd>
            @{
                List<string> states = (List<string>)ViewData["AllStates"];
            }
            @Html.DropDownList("State", states.Select(s => new SelectListItem() { Text = s, Value = s, Selected = (s == Model.State) }))
        </dd>
    </dl>

    <p>
        <input type="submit" value="登録" />
        <input type="button" id="btnCancel" value="キャンセル" />
    </p>
    @Html.ValidationSummary("入力にエラーがあります。修正してください。")
}

<hr />

<p>
    <a href="/">業務メニューに戻る</a>
</p>


@section Scripts {

    <script type="text/javascript">
    $(function () {
        $("#btnCancel").click(function () {
            window.location = "@Url.Action("ListAuthors")";
            return false;
        });
    });
    </script>
}
```

Sample02Controllerに編集画面からPOSTされたときのアクションメソッドを実装します。

```cs
[HttpPost]
public ActionResult EditAuthor(string id, AuthorEditViewModel model)
{
    using (var pubs = new PubsEntities())
    {
        var query = pubs.Authors.Select(a => a.State).Distinct();
        ViewData["AllStates"] = query.ToList();
    }

    // 送信されてきたデータを再チェック
    if (ModelState.IsValid == false)
    {
        // 前画面を返す
        // ID フィールドがロストしているので補完する
        model.AuthorId = id;
        return View(model);
    }

    model.AuthorId = id;

    // データベースに登録を試みる
    using (var pubs = new PubsEntities())
    {
        var target = pubs.Authors.Where(a => a.AuthorId == model.AuthorId).FirstOrDefault();
        target.AuthorFirstName = model.AuthorFirstName;
        target.AuthorLastName = model.AuthorLastName;
        target.Phone = model.Phone;
        target.State = model.State;

        pubs.SaveChanges();
    }
    // 一覧画面に帰る
    return RedirectToAction("ListAuthors");
}
```