# Scorpio-CSharp #
* author : 杨林远
* email : qingfeng346@126.com
* qq : 250178206

## ps: 此脚本本来是用作Unity热更新使用的脚本,纯c#实现 基于.net2.0  兼容所有c#平台 后续正在移植到java

## v0.0.2beta (2014-10-13) ##
-----------
* 修复已知BUG
* 增加对不定参的支持  
    示例：  
        function hello(a,...args) { }    
    args会传入一个Array
* 增加 eval函数 可以动态执行一段代码
* 删除对ulong类型的支持
* 增加基础for循环 for(i=begin,finished(包含此值),step)  
    示例：  
        for (i=0,10000)  
        {  
        }  
        for (i=0,10000,2)  
        {  
        }
* 统一scriptobject产出函数 方便以后加入对象池
* 增加Unity例子 (亲测支持pc web android ios wp(需要修改一些基础函数调用,应该不影响功能使用,稍后会发布一个版本支持wp))