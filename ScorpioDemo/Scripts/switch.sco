//swith 目前支持 number 和 string类型 且不支持变量  只支持常量
function sw(a)
{
    switch (a)
    {
        case 1:
        case 2:
            print("1 or 2")
            break;
        case "a":
        case "b":
            print("a or b")
            break;
        default:
            print("default")
            break;
    }
}
sw(1)
sw(2)
sw("a")
sw("b")
sw("scorpio")