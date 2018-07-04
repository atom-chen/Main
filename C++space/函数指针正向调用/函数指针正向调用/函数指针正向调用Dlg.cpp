
// 函数指针正向调用Dlg.cpp : 实现文件
//

#include "stdafx.h"
#include "函数指针正向调用.h"
#include "函数指针正向调用Dlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 对话框数据
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// C函数指针正向调用Dlg 对话框




C函数指针正向调用Dlg::C函数指针正向调用Dlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(C函数指针正向调用Dlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void C函数指针正向调用Dlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(C函数指针正向调用Dlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &C函数指针正向调用Dlg::OnBnClickedButton1)
END_MESSAGE_MAP()


// C函数指针正向调用Dlg 消息处理程序

BOOL C函数指针正向调用Dlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void C函数指针正向调用Dlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void C函数指针正向调用Dlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR C函数指针正向调用Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


//函数指针类型  
//客户端初始化 获取handle上下
typedef int (*CltSocketInit)(void **handle /*out*/); 
	
//客户端发报文
typedef int (*CltSocketSend)(void *handle /*in*/, unsigned char *buf /*in*/,  int buflen /*in*/);

//客户端收报文
typedef int (*CltSocketRev)(void *handle /*in*/, unsigned char *buf /*in*/, int *buflen /*in out*/);
	
//客户端释放资源
typedef int (*CltSocketDestory)(void *handle/*in*/);
//------------------第一套api接口---End-----------------------------------//
	

void C函数指针正向调用Dlg::OnBnClickedButton1()
{
	// TODO: 在此添加控件通知处理程序代码
	AfxMessageBox("ddddd");


	HINSTANCE hInstance;
	CltSocketInit		cltSocketInit;  //用函数指针类型 定义 函数指针变量 
	CltSocketSend		cltSocketSend;
	CltSocketRev		cltSocketRev;
	CltSocketDestory	cltSocketDestory;


	//找函数的入口地址  
	hInstance=::LoadLibrary("c:/socketclient.dll");
	cltSocketInit =(CltSocketInit)::GetProcAddress(hInstance, "cltSocketInit");
	cltSocketSend =(CltSocketSend)::GetProcAddress(hInstance, "cltSocketSend");
	cltSocketRev =(CltSocketRev)::GetProcAddress(hInstance, "cltSocketRev");
	cltSocketDestory =(CltSocketDestory)::GetProcAddress(hInstance, "cltSocketDestory");

	void			*handle = NULL;
	unsigned char	 buf[2048];
	int				buflen = 9;
	unsigned char	out[2048];
	int				outlen;
	int				ret = 0;

	strcpy((char *)buf, "aaaabbbdddddddddddddsssssssssss");

	ret = cltSocketInit(&handle);
	ret =  cltSocketSend(handle, buf, buflen);

	ret = cltSocketRev(handle, out, &outlen);

	ret = cltSocketDestory(handle);

}
