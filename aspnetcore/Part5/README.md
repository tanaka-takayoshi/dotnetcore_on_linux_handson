### Part5

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

///TBD ブラウザ側の実装