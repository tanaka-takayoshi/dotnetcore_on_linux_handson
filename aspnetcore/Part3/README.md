# Part3 ASP.NET MVC Core 1.0 の基本的な使い方

#### MVCランタイムの追加

MVCの機能を追加していくことにします。``Startup.cs``を編集し、MVCのランタイムを追加します。

```cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
```

次にシンプルなControllerとViewを追加します。yoコマンドを使ってテンプレートクラスを作成しますが、出力先となるフォルダを予め作っておく必要があります。

```bash
$ mkdir Controllers
$ yo aspnet:MvcController HomeController

$ mkdir Controllers
$ yo aspnet:MvcView Home/Index
```

HomeController.csはこのようにします。

```cs
csusing System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCorehandson.Models;
using AspNetCorehandson.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCorehandson.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

Home/Index.cshtmlはいったんこのような静的な状態にします。

```html
<!DOCTYPE html>
<html>
<head>
    <title>著者データ一覧</title>
</head>
<body>
    これからコードを書く...
</body>
</html>
```

ここまで作成してF5でデバッグ起動すると次のように表示されるはずです。
![](https://raw.githubusercontent.com/tanaka-takayoshi/dotnetcore_on_linux_handson/master/aspnetcore/images/part3/01First.png)

#### データアクセスロジックとVMの追加

次にデータアクセスロジックを記述し、VMを追加します。データアクセスロジックは今回は簡単のためにControllerに直書きすることにします。取得したデータはViewModelのプロパティにつめて、Viewに渡します。これにより、Razorの編集の際にViewModelの型を指定して入力支援が受けられる、のがVisual StudioですがVisual Studio Codeでは現状Viewの入力支援はありませんでした。しかし、Viewのコンパイルによる型チェックが可能であるため、ViewModelを利用することをお勧めします。

まずViewModelクラスを作ります。yoコマンドでクラスのテンプレートも作成できますが、あらかじめ作成するディレクトリに移動している必要があります。

```bash
$ mkdir ViewModels
$ cd ViewModels
$ yo aspnet:Class AuthorListViewModel
```

```cs
using AspNetCorehandson.Models;

namespace AspNetCorehandson.ViewModels
{
    public class AuthorListViewModel
    {
        public Author[] Authors { get; set; }
    }
}
```

Controllerクラスにデータアクセスロジックを追加します。ViewModelのインスタンスはViewメソッドの引数で渡します。
```cs
        public IActionResult Index()
        {
            var vm = new AuthorListViewModel();
            using(var pubs = new PubsEntities())
            {
                vm.Authors = pubs.Authors.ToArray();
            }
            return View(vm);
        }
```

Home/Index.cshtml はこのように記述します。
```html
@model AspNetCorehandson.ViewModels.AuthorListViewModel

<!DOCTYPE html>
<html>
<head>
    <title>著者データ一覧</title>
</head>
<body>
    <table>
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
</body>
</html>
```

ここまで作成して、F5でデバッグ起動すると、このような表示になるはずです。
![](https://raw.githubusercontent.com/tanaka-takayoshi/dotnetcore_on_linux_handson/master/aspnetcore/images/part3/02Second.png)
