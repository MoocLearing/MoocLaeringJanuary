﻿
1.dropdownlist试了很多遍都用不出来，在别的项目上可以通过user表与country表FK相联实现这个功能。

2.login的index.html上username和password加上validation的话，在后台action里modelstate.IsValid永远是false

3.之前用session存登录user的对象，里面有user的roletype可以进行filter的操作，但cookie存在browser端如何
得到用户的roletype信息？或者如果把这个信息存入cookie是否安全？

01/15  
1.从数据库里添加的user数据读不出来，必须建立EF controller用create action添加的才能读出来,并且codefirst的数据不能从数据库修改

2.当使用user/index里的一个user选择edit时，后台代码通过点击的这个userID得到user对象，之后把user对象mapper成
AdminUserViewModel对象，但最后一个TeacherId系统要求是IEnumerable<SelectListItem>类型，这个不知道如何转换好了？？
之前在关联表实现drop down list时候是可以直接做两个对象的mapper的。

3.在admin域里的filter里想通过session的username值得到user的对象从而判断roletype的值是否为管理员，但总是报一个异常：'Object reference not set to an instance of an object


01/25
如果一个action接受的是前端来的Ajax发出的json，那么返回值必须是retrun json()吗？还是可以直接return view？

为什么CategoryViewModel要在构造函数里面初始化一个CategoryList对象？

account/login 里面, return Json(new {code=0}) 结果直接就打印在了login.html上而没有执行success回调函数.

为什么@html.validation在createa界面里面不管用了？用一个空的@mode Mooc.MoocData.Entity.Teacher 作为@html.validation的基础不行吗？


02/10

想把Course和chapter在一个list中显示，chapter的groupby按照courseID来排序。但如果要使用真分页，每次还是要查找全部的chapters数据table才能
进行groupby和orderby，也就失去了真分页的意义了

MongoDB还在研究如何使用在这个项目中存放和读取video，目前video是假设存在本地的文件路径。

02/13
点击改变当前course的statue的button功能只能绑定一个对象，若要动态绑定onclick对象没有实现

点击显示当前course的chapter功能，success回调函数没有接受到返回的Json对象

02/16
对于用enum类来编译int属性的问题：
模仿course里的viewmodel写出teacher里的编译enum的viewmodel，目的是列出的teacher对象显示在HTML上的是teacherdepartment的文字，
而不是数字。
public string TeacherDepartmentToString => Enum.GetName(typeof(TeacherDepartmentEnum), TeacherDepartment);
在HTML里读取的传来的TeacherViewModel中的TeacherDepartmentToString结果为undefined？？？

02/20
当修改用户时候，如果不上传图片的情况下，把“ImgGuid”: @modle.ImgGuid 参数传回action里，前端submit这里直接报错。
不知道如何解决上传更新图片不上传保持原始图片这样的分支操作？？？EditCrouse和EditUser都是这个问题

03/05
后台的admin/account/login的登录HTML里面的HTML代码里面有问题，当ajax在action里成功返回view时候，
永远是显示在页面上{code=0}或{code=1,msg="错误"}这样的信息。我把前台login拿过来一部分一部分做测试，
发现action里的login方法，和html里的ajax都没错，但就是html代码不一样就出现这样情况。
花了很久的时间，实在找不到什么问题，只能将前台login代码直接拿到后台来用了。

12/03
在HTML：
function initCourse() {
        $.ajax({
            url: "@Url.Content("~/Home/_PartCourseList")",
            type: "POST",
            dataType: 'html',
            success: function(data) {
                $("#dataBind").html(data);
                console.log("course列表ajax success执行");
            }
        });
    }

在controller：
public ActionResult _PartCourseList()
    {
        var courses = _dataContext.Courses.Where(x=>x.Status==1).ToList();
        return PartialView(courses);
    }

为什么controller里的返回partialView的action，能把整个partialview的HTML渲染完成的页面+参数当成一个整体的data返回给AJAX？