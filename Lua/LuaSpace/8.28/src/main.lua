a="one string";
b=string.gsub(a,"one","another");
print(b); 


c="I\"m a student";
print(c); 


page=[[
<html><head><body><body/><head/><html/>
]]

print(page);

a="19"+1;
print(a);

print(a.."addadasd");

a={};
print(type(a));
k="x";
a[k]=10;
print(k.."="..a[k]);

a["x"]=10;
b=a;
a=nil;
print(b["x"]);

for i=1,#b do
print(a[i]);
end;

b[#b]=nil;
b[#b+1]=3000;
print(b[#b]);

i=10;
j="10";
k="+10";
b[i]="one value";
b[j]="another value";
b[k]="yet anther";
print(b[i]);
print(b[tonumber(j)])
print(b[k]);







