���Դ���Ŀ�׸��ҵĶ��� ���ȷ� ��ף����һ��ͯ�ڿ��� -- jjx


DotNetTools Workflow�����¼��workflow)����ֵ�� osworkflow (http://www.opensymphony.com/osworkflow )�����ڵ�һ�׶ε���ֵĿ������� www.gotdotnet.com/workspaces/messageboard/thread.aspx?id=6666274d-a3e5-4e7b-b0cb-368dbb2c4bca&threadid=c120ce1d-9b23-475c-bcff-c1778d5c22fe

�ڶ����׶ε�Ǩ��Ŀ������� http://www.gotdotnet.com/workspaces/messageboard/thread.aspx?id=6666274d-a3e5-4e7b-b0cb-368dbb2c4bca&threadid=e58709d7-5625-49bf-9259-2664bb8de75e



��Ŀ��workspace �� workspaces.gotdotnet.com/osworkflow����Ŀʹ��apache license 2.0

�ð汾Ϊpublic preview, ��Ҫ���ڹ������ǵĽ��Ȳ����������������֪���������Releasenotes.txt �ļ�

����Ŀ����

workflow ��Ŀ�������¿�Դ��Ŀ

spring.net http://www.springframework.net
nhibernate http://nhibernate.souceforge.net
log4.net http://log4net.apache.net
nunit http://nunit.sourceforge.net
nant 0.85  http://nant.sourceforge.net


����Spring.net ��Nhibernate��������μ� http://www.gotdotnet.com/workspaces/messageboard/thread.aspx?id=6666274d-a3e5-4e7b-b0cb-368dbb2c4bca&threadid=a4d7d819-2372-4b77-a185-d70f63b4aa32



����Ŀ���


workflow ��Ŀ��������������Ŀ���

DotNetTools.Core �ṩһЩʵ����
DotNetTools.PropertySet ��ֵ�� www.opensymphony.com/propertyset
DotNetTools.Workflow ����Ŀ
DotNetTools.Test ������Ŀ

��ĿĿ¼�ṹ����

workflow
   lib  �����ĵ��������
   samples ���в�������Ĺ����������ļ�����
  src
     DotNetTools.Workflow
     DotNetTools.Core
     DotNetTools.PropertySet
     DotNetTools.Test

   etc  - һЩ���ݿ�ű������������ļ��� 
  build  ��ű������ļ�


����������ʵ��������propertyset adoʵ�֣����ݿ�

�½�һ����Ϊosworkflow ���ݿ�
�����ʹ��nhibernate ��������etc/workflow_hibernate.sql ������������etc/workflow.sql���ڵ�ǰ�Ĳ����У�������AdoPropertySet�Ĳ��ԣ���ˣ�������ȴ��������ݿ�



����Դ�������- ʹ��nant

nant -t:net-1.1 debug  ����debug�汾
nant -t:net-1.1 release ����release�汾

nant -t:net-1.1 clean ���buildĿ¼ 
nant -t:net-1.1 cleanall ���buildĿ¼��vs.net��reshaper ���ɵ�Ŀ¼���ļ�(�� bin/obj)
nant -t:net-1.1 runtests �������в���



����Դ������� -  ʹ��vs.net 


�ٴ� vs.net 2003
�ڴ� workflow\src Ŀ¼�µ� DotNetTools.sln �ļ�
�۱���



����vs.net �����в��� 

���¾�ָDotNetTools.Test��Ŀ

��ȷ�����build���Ŀ¼�´���propertyset.xml�ļ������û������Խ�etc �µ�propertyset.xml ���Ƶ�DotNetTools.Test��Ŀ��build���Ŀ¼
�������ʵ������޸� AdoPropertySet�����е�connectionString

��ȷ�����build���Ŀ¼�´���samplesĿ¼����������ڣ��뽫samplesĿ¼�������Ƶ�DotNetTools.Test��Ŀ��build���Ŀ¼
��ȷ�����build���Ŀ¼�´���dotnettools.test.dll.config�ļ�����������ڣ��뽫etc/DotNetTools.test.dll.config���Ƶ���Ŀ¼
������dotNetTools.Test��Ŀ�еĲ��ԣ�Ĭ�ϵ�workflowStoreʹ�õ�MemoryWorkflowStore




������SqlClientWorkflowStore


���޸�DotNetTools.Test�е�workflow.xml �ļ�����persistence �޸�Ϊ(ע��������ʵ���������connectionString��������


<persistence type="DotNetTools.Workflow.Spi.Ado.SqlClientWorkflowStore,DotNetTools.Workflow">
	<property key="connectionString" value="server=(local);user id=sa;pwd=;database=osworkflow"/>
</persistence>


������Ҫrebuild������build��Ƕ����Դ��Ч
�����в���


������HibernateWorkflowStore


���޸�DotNetTools.Test�е�workflow.xml �ļ�����persistence �޸�Ϊ(ע��������ʵ���������connectionString�������ã��������ʵ�������������(hibernate.connection.connection_string)

<persistence type="DotNetTools.Workflow.Spi.Hibernate.HibernateWorkflowStore,DotNetTools.Workflow">
	<property key="hibernate.connection.provider" value="NHibernate.Connection.DriverConnectionProvider"/>
	<property key="hibernate.dialect" value="NHibernate.Dialect.MsSql2000Dialect"/>
	<property key="hibernate.connection.driver_type" value="NHibernate.Driver.SqlClientDriver"/>
	<property key="hibernate.show_sql" value="true"/>
	<property key="hibernate.use_outer_join" value="true"/>
	<property key="hibernate.connection.connection_string" value="Server=localhost;initial catalog=osworkflow;User ID=sa;Password=;Min Pool Size=2"/>
</persistence>


������Ҫrebuild������build��Ƕ����Դ��Ч
�����в���


��ʹ��log4.net


etc ��DotNetTools.test.dll.config�ṩ��һ�����ã�����Խ���ŵ�DotNetTools.Test��Ŀ�� build���Ŀ¼
���Ի����ϸ����־��Ϣ
