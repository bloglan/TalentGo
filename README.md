TalentGO Project
===

## 概述
TalentGo是一个招聘报名系统。

## 项目框架
* TalentGoCore 招聘报名业务逻辑代码。
* TalentGoServerLib 招聘报名系统服务器相关逻辑。
* TalentGoEntityFramework 实现数据库访问。
* TalentGoWebApp 供求职者办理业务的网站。
* TalentGoManagerWebApp 供人力资源部门审核和管理报名资料的网站。

各个工程对应Tests为后缀的工程为单元测试。


## 代码解读

### 指导原则和思想
从整个项目上看，我们采用以领域驱动设计为原则，充分将业务逻辑、UI逻辑、存储逻辑等各种逻辑解耦，使用单元测试确保业务行为是预期的。通过这样一些手段，确保系统在投入运行以后，仍然有极高的可维护性、扩展性。

### 管理器设计模式
业务逻辑部分，我们采用了管理器设计模式。比如，填写报名表，那么我们就有了“报名表”这样一个领域对象（ApplicationForm），除了用户填写的报名信息，还存储着审核信息和相关控制字段，我们需要一个“审核”动作来负责控制这些字段，确保不会在其他地方修改他们的值，因此，我们对应创建了报名表管理器（ApplicationFormManager），调用管理器的“审核”方法并传入审核所需的参数，由管理器负责检查条件、变更报名表的审核控制字段并自动更新到数据库。

你应该注意到了，初始化ApplicationFormManager的时候需要传入实现IApplicationFormStore接口的对象，这个Store对象负责报名表的CRUD持久化操作。当然，无论你希望把对象持久化到什么地方（数据库、文件系统），这都没问题，只要实现该接口，并且传给Manager即可。

这样，我们就达成了业务逻辑和持久化之间的解耦。我们在TalentGoEntityFramework工程里开发了IApplicationFormStore接口的实现，采用EntityFramework框架将领域对象持久化到SQL Server数据库。

### ASP.NET MVC

ASP.NET MVC是一套由官方出品的非常优秀的MVC框架。相比WebForm模式，MVC更符合面向对象原则，模型视图分离，更加具备可测试性。

MVC中的Model，其实并非等同于领域模型，比如填写报名表时，我只需要用户填写需要填写的部分，控制字段是不需要填写，也不允许修改的，那么，我们就需要一个ASP.NET所用的Model模型来设计表单，在返回数据以后，再选择合适的方法更新到领域模型。

### 依赖倒转和IoC

那么在什么时机初始化ApplicationFormManager并且给他传递合适的Store呢，这里我们遵循了依赖倒转的思想，采用Autofac这种非常流行的IoC（依赖注入）框架来进行。

在以往，依赖倒转通常需要使用Factory设计模式，但是不可避免在Factory中出现耦合。现在，我们有Autofac等优秀的IoC框架，实现依赖倒转就变得更加优雅！

以本项目为例，用户发起HTTP Request请求后，ASP.NET MVC负责调用依赖解析器（DependencyResolver）初始化相应的Controller，调用相应的方法来返回视图。
ASP.NET MVC默认的依赖解析器要求Controller必须具有一个无参数构造函数。

但是，你可以看到ApplicationFormController有一个需要传入ApplicationFormManager参数的构造函数，并没有无参数的构造函数。它是如何被正确初始化呢？

要点就在于，我们需要替换依赖解析器，以便依赖解析器能够正确初始化Controller。这个工作放在Global.ascx.cs中进行，也就是说，我们需要在Application_Start_时把这个工作做掉。

因此，你会看到，在Global.ascx中，配置AppBuilder建造器，把所需的资源告诉Autofac，然后使用建造器创建IoC容器，使用IoC容器初始化Autofac提供的依赖解析器，然后替换ASP.NET MVC的默认依赖解析器。

因此，当初始化ApplicationFormController时，Autofac的依赖解析器知道去哪里找到ApplicationFormManager类初始化它，然后传递给Controller，以完成后续工作。

它的工作如此巧妙，你几乎不需要开发紧耦合的代码，就完成了依赖倒转。

### 单元测试
单元测试是写一段调用目标方法的代码，在系统整体运行前，通过测试代码调用目标方法，并探测方法执行后是否获得预期结果。

单元测试的重要意义在于用程序的手段指导和检测一个方法的实现是否满足预期结果。




祝编码愉快!