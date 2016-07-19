Part4

```
$ yo aspnet:MvcController Sample01Controller
$ mkdir Views/Sample01
$ yo aspnet:MvcView Sample01/ShowAllAuthors
$ yo aspnet:MvcView Sample01/ShowAuthorsByState
```


```
[HttpGet]
        public IList<Author> GetAllAuthors()
        {
            using (var pubs = new PubsEntities())
            {
                return pubs.Authors.ToArray();
            }
        }
        ```