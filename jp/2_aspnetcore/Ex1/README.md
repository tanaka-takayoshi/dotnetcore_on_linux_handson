# Ex1 Tag Helper の導入と Anti Forgery

ASP.NET Core で Razor に Tag Helperという新機能が導入されました。今迄、``@Html.BeginFrom``というように記述していたものを、HTMLのタグ形式で記述できるようになる機能です。これを使ってSample02のEditAuthor.cshtmlを書き直してみることにします。

Tag Helperを導入するために、 Views/_ViewImports.cshtml を作成し、以下のように記述します。

```html
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

これにより全てのViewでTag Helperが有効になります。
次にEditAuthor.cshtmlを次のように書き直します。

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

<form asp-controller="Sample02"
      asp-action="EditAuthor"
      method="post"
      id="@Model.AuthorId">
    <dl>
        <dt>著者ID</dt>
        <dd>@Model.AuthorId</dd>
    </dl>
    <dl>
        <dt>著者名（名）</dt>
        <dd><input asp-for="AuthorFirstName"/><span asp-validation-for="AuthorFirstName"/></dd>
    </dl>
    <dl>
        <dt>著者姓（姓）</dt>
        <dd><input asp-for="AuthorLastName"/><span asp-validation-for="AuthorLastName"/></dd>
    </dl>
    <dl>
        <dt>電話番号</dt>
        <dd><input asp-for="Phone"/><span asp-validation-for="Phone"/></dd>
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
    <div asp-validation-summary="All">入力にエラーがあります。修正してください。</div>
</form>

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

``<form asp-controller="Sample02">`` といったようによりHTMLに近い形式で記述できるようになりました。DropDownListはデフォルトでタグは用意されていないのでそのままにしています。

ここで、CSRF攻撃への対処として、POSTメソッドに``ValidateAntiForgeryToken``属性をつけることにしましょう。これによりフォームを生成するときにワンタイムトークンを発行しHidden値に書き込まれます。Tag Helperを利用する場合は、送信先のメソッドに``ValidateAntiForgeryToken``属性をつけるだけでOKです。

```cs
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult EditAuthor(string id, AuthorEditViewModel model)
{
// 以下略
```