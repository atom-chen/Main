-- Generate Code,do not edit

--
local t = {
    FailedIfHold = 0, -- 被占了则创建失败
    Replace = 1, -- 替换之前创建的
    Overlay = 2, -- 继续创建在改位置重叠
    Update = 3, -- 更新数据
    
}
return t