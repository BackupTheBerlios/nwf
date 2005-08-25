谨以此项目献给我的儿子 蒋奕帆 ，祝你六一儿童节快乐 -- jjx


DotNetTools Workflow（以下简称workflow)，移值自 osworkflow (http://www.opensymphony.com/osworkflow )，关于第一阶段的移值目标请参阅 www.gotdotnet.com/workspaces/messageboard/thread.aspx?id=6666274d-a3e5-4e7b-b0cb-368dbb2c4bca&threadid=c120ce1d-9b23-475c-bcff-c1778d5c22fe

第二个阶段的迁移目标请参阅 http://www.gotdotnet.com/workspaces/messageboard/thread.aspx?id=6666274d-a3e5-4e7b-b0cb-368dbb2c4bca&threadid=e58709d7-5625-49bf-9259-2664bb8de75e



项目的workspace 在 workspaces.gotdotnet.com/osworkflow，项目使用apache license 2.0

该版本为public preview, 主要用于公布我们的进度并吸收意见，部分已知问题请参阅Releasenotes.txt 文件

■项目需求

workflow 项目依赖以下开源项目

spring.net http://www.springframework.net
nhibernate http://nhibernate.souceforge.net
log4.net http://log4net.apache.net
nunit http://nunit.sourceforge.net
nant 0.85  http://nant.sourceforge.net


关于Spring.net 和Nhibernate依赖，请参见 http://www.gotdotnet.com/workspaces/messageboard/thread.aspx?id=6666274d-a3e5-4e7b-b0cb-368dbb2c4bca&threadid=a4d7d819-2372-4b77-a185-d70f63b4aa32



■项目组成


workflow 项目有以下三个子项目组成

DotNetTools.Core 提供一些实用类
DotNetTools.PropertySet 移值自 www.opensymphony.com/propertyset
DotNetTools.Workflow 本项目
DotNetTools.Test 测试项目

项目目录结构如下

workflow
   lib  依赖的第三方组件
   samples 运行测试所需的工作流配置文件例子
  src
     DotNetTools.Workflow
     DotNetTools.Core
     DotNetTools.PropertySet
     DotNetTools.Test

   etc  - 一些数据库脚本，例子配置文件等 
  build  存放编译后的文件


■创建流程实例（包括propertyset ado实现）数据库

新建一个名为osworkflow 数据库
如果你使用nhibernate ，请运行etc/workflow_hibernate.sql ，否则，请运行etc/workflow.sql，在当前的测试中，包括对AdoPropertySet的测试，因此，你务必先创建该数据库



■从源代码编译- 使用nant

nant -t:net-1.1 debug  创建debug版本
nant -t:net-1.1 release 创建release版本

nant -t:net-1.1 clean 清除build目录 
nant -t:net-1.1 cleanall 清除build目录和vs.net及reshaper 生成的目录和文件(如 bin/obj)
nant -t:net-1.1 runtests 单独运行测试



■从源代码编译 -  使用vs.net 


①打开 vs.net 2003
②打开 workflow\src 目录下的 DotNetTools.sln 文件
③编译



■在vs.net 中运行测试 

以下均指DotNetTools.Test项目

①确认你的build输出目录下存在propertyset.xml文件，如果没有你可以将etc 下的propertyset.xml 复制到DotNetTools.Test项目的build输出目录
根据你的实际情况修改 AdoPropertySet配置中的connectionString

②确认你的build输出目录下存在samples目录，如果不存在，请将samples目录整个复制到DotNetTools.Test项目的build输出目录
③确认你的build输出目录下存在dotnettools.test.dll.config文件，如果不存在，请将etc/DotNetTools.test.dll.config复制到该目录
③运行dotNetTools.Test项目中的测试，默认的workflowStore使用的MemoryWorkflowStore




■测试SqlClientWorkflowStore


①修改DotNetTools.Test中的workflow.xml 文件，将persistence 修改为(注意根据你的实际情况调整connectionString调整设置


<persistence type="DotNetTools.Workflow.Spi.Ado.SqlClientWorkflowStore,DotNetTools.Workflow">
	<property key="connectionString" value="server=(local);user id=sa;pwd=;database=osworkflow"/>
</persistence>


②你需要rebuild而不是build你嵌入资源生效
③运行测试


■测试HibernateWorkflowStore


①修改DotNetTools.Test中的workflow.xml 文件，将persistence 修改为(注意根据你的实际情况调整connectionString调整设置，根据你的实际情况调整配置(hibernate.connection.connection_string)

<persistence type="DotNetTools.Workflow.Spi.Hibernate.HibernateWorkflowStore,DotNetTools.Workflow">
	<property key="hibernate.connection.provider" value="NHibernate.Connection.DriverConnectionProvider"/>
	<property key="hibernate.dialect" value="NHibernate.Dialect.MsSql2000Dialect"/>
	<property key="hibernate.connection.driver_type" value="NHibernate.Driver.SqlClientDriver"/>
	<property key="hibernate.show_sql" value="true"/>
	<property key="hibernate.use_outer_join" value="true"/>
	<property key="hibernate.connection.connection_string" value="Server=localhost;initial catalog=osworkflow;User ID=sa;Password=;Min Pool Size=2"/>
</persistence>


②你需要rebuild而不是build你嵌入资源生效
③运行测试


■使用log4.net


etc 的DotNetTools.test.dll.config提供了一个配置，你可以将其放到DotNetTools.Test项目的 build输出目录
，以获得详细的日志信息
