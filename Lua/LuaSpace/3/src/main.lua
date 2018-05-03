
print(4 and false);
print(4 or false);

x=4;y=5;
max=(x>y) and x or y;
print(max);

print(0 .. 1);

day={"Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"};

a={x=10,y=20};
print(a.x);

w={x=0,y=10,label="console"};
x={math.sin(0),math.sin(1),math.sin(2)};

w[1]="another fild";
w.x=nil;

--[[
list=nil;
for line in io.lines() do
list={next=list,value=line}
end


local l=list
while l do
print(l.value)
l=l.next;
end;]]


polyline={color="blue",thickness=2,npoints=4,
{x=0,y=0},
{x=-10,y=0},
{x=0,y=1}};
print(polyline[2].x);

opnames={["+"]="add",["-"]="sub",["*"]="mul",["/"]="div"};
i=20;s="-";
a={[i+0]=s,[i+1]=s..s,[i+2]=s..s..s};
print(opnames[s]);









