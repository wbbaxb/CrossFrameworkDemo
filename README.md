# CrossFrameworkDemo

.NET 8.0调用.NET Framework方法的简单Demo，展示了不同的跨框架调用场景和方法。

## 项目结构

- **CrossFrameworkDemo**：使用.NET 8.0开发的WPF客户端应用程序
- **CrossFrameworkLibrary**：使用.NET Framework 4.5开发的类库

## 实现方式与场景演示

本Demo展示了在.NET 8.0中调用.NET Framework方法的多种方式，以及不同场景下的适用性比较：

### 1. 直接引用方式

通过项目引用直接调用.NET Framework库中的方法：

> 适用于直接调用无Framework特有API的方法

```csharp
AccessTest.GetPublicMessage();
```

### 2. 反射方式

通过反射动态加载.NET Framework程序集并调用其中的方法

> 和直接引用不同的是这种方式可以访问程序集内部类或方法

```csharp
Assembly appAssembly = Assembly.LoadFrom(dllPath);

Type type = appAssembly.GetType("CrossFrameworkLibrary.AccessTest");

MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public |
    BindingFlags.NonPublic | BindingFlags.Static);

methodInfo.Invoke(null, new object[] {});
```

#### 反射可行而直接引用不可行的情况

以下情况中，反射可以成功调用而直接引用无法调用：

- 访问程序集内部类或方法
- 动态加载的程序集


### 3. DllImport方式 (P/Invoke)

通过P/Invoke直接调用导出的函数：

```csharp
[DllImport("CrossFrameworkLibrary.dll", EntryPoint = "RemotingTest",
        CallingConvention = CallingConvention.Cdecl)]
public static extern string RemotingTest();
```


#### DllExport使用流程

DllExport用于将.NET方法导出为标准的Windows DLL导出函数，使其可以被任何支持调用DLL的语言使用。

1. 运行解决方案根目录中的DllExport.bat
2. 在弹出的界面中选择需要配置DllExport的项目（本例中是CrossFrameworkLibrary）, 勾选Installed , 点击Apply
3. 重新加载solution
4. 在要导出的方法上添加DllExport特性：
```csharp
[DllExport("GetPublicMessage", CallingConvention.Cdecl)]
public static string GetPublicMessage()
{
    return "This is a public message!";
}
```
5. 编译项目，生成可供其他语言调用的DLL


> 本项目使用的DllExport.bat来自 [3F/DllExport](https://github.com/3F/DllExport)

##### 高级选项

- 指定导出名称：`[DllExport("ExportedName")]`
- 指定调用约定：`[DllExport(CallingConvention.Cdecl)]`
- 组合使用：`[DllExport("Init", CallingConvention.StdCall)]`

## 方法对比与推荐

| 方法 | 适用场景 | 优点 | 缺点 | 兼容性 |
|------|----------|------|------|--------|
| 直接引用 | 简单方法，无Framework特有API | 使用简单，IDE支持完整 | 只适用于简单场景，兼容性低 | 低 |
| 反射 | 中等复杂度方法，部分Framework API | 灵活，运行时动态加载 | 性能差，错误难排查 | 中 |
| DllImport | 所有方法，包括复杂的Framework特有功能 | 性能好，类型安全 | 需要匹配导出函数，接口固定 | 高 |

## 推荐方式

**推荐使用DllImport方式**，原因如下：
1. **最高的兼容性** - 几乎可以调用任何.NET Framework的功能，包括Windows Forms和特定的Framework API
2. **性能最好** - 直接调用导出函数，避免了反射的性能开销
3. **类型安全** - 在编译时即可检查类型错误

## Python调用示例

项目根目录下的`call_dotnet_dll.py`脚本展示了如何通过Python调用DLL导出的函数：

使用方法：
1. 确保已编译好CrossFrameworkLibrary.dll
2. 运行Python脚本：`python call_dotnet_dll.py`

## 许可证

本项目使用MIT许可证。详细信息请参阅[LICENSE](LICENSE)文件。
