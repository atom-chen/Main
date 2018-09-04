#pragma once

#include <Windows.h>
#include <tchar.h>
#include <EGL/egl.h>
#include <gles2/gl2.h>
#include "CELLShader.hpp"
#include "CELLMath.hpp"

#include "freeImage/FreeImage.h"

class   CELLWinApp
{
public:
    /**
    *   Ӧ�ó���ʵ�����
    */
    HINSTANCE           _hInstance;
    /**
    *   ���ھ������������ʹ��
    */
    HWND                _hWnd;
    /**
    *   ���ڵĿ�Ⱥ͸߶�
    */
    int                 _winWidth;
    int                 _winHeight;
    EGLConfig			_config;
    EGLSurface 			_surface;
    EGLContext 			_context;
    EGLDisplay 			_display;
public:
    CELLWinApp(HINSTANCE hInstance = 0)
    {
        _hWnd       =   0;
        _winWidth   =   0;
        _winHeight  =   0;
        _hInstance  =   hInstance;
        _config     =   0;
        _surface    =   0;
        _context    =   0;
        _display    =   0;
        /**
        *   Ҫ�봴��һ�����ڣ�����Ҫע��һ��������
        *   ����ڴ棬�����˽�windows��ɣ����ﲻ�����ܡ�
        */
        ::WNDCLASSEX winClass;
        winClass.lpszClassName  =   _T("CELLWinApp");
        winClass.cbSize         =   sizeof(::WNDCLASSEX);
        winClass.style          =   CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
        winClass.lpfnWndProc    =   windowProc;
        winClass.hInstance      =   hInstance;
        winClass.hIcon	        =   0;
        winClass.hIconSm	    =   0;
        winClass.hCursor        =   LoadCursor(NULL, IDC_ARROW);
        winClass.hbrBackground  =   (HBRUSH)GetStockObject(BLACK_BRUSH);
        winClass.lpszMenuName   =   NULL;
        winClass.cbClsExtra     =   0;
        winClass.cbWndExtra     =   0;
        RegisterClassEx(&winClass);
    }
    virtual ~CELLWinApp()
    {
    }
    /**
    *   ��Ⱦ����
    */
    virtual void    render()
    {
    }

    /**
    *   ��ں���
    *   width :�������ڿ��
    *   height:�������ڵĸ߶�
    */
    int     start(HWND hWnd,int width,int height)
    {
        _winWidth   =   width;
        _winHeight  =   height;

        /**
        *   ��������
        */
        if (hWnd == 0)
        {
            if (!_createWindow(_winWidth,_winHeight))
            {
                return  -1;
            }
        }
        else
        {
            _hWnd   =   hWnd;
        }
        /**
        *   ��ʼ��gles������
        */
        if (!initDevice())
        {
            return  -2;
        }
        onInit();
       
        if (hWnd)
        {
            return  0;
        }
        /**
        *   ������Ϣѭ��
        */
        MSG msg =   {0};
        while(msg.message != WM_QUIT)
        {
            if (msg.message == WM_DESTROY || 
                msg.message == WM_CLOSE)
            {
                break;
            }
            /**
            *   ����Ϣ��������Ϣ������Ϣ���������Ⱦ����
            */
            if( PeekMessage( &msg, NULL, 0, 0, PM_REMOVE ) )
            { 
                TranslateMessage( &msg );
                DispatchMessage( &msg );
            }
            else
            {
                render();
            }
        }
        /**
        *   �ر�
        */
        shutDownDevice();

        return  0;
    }
    /**
    *   ��ʼ��OpenGL
    */
    bool    initDevice()
    {

        const EGLint attribs[] =
        {
            EGL_SURFACE_TYPE, EGL_WINDOW_BIT,
            EGL_BLUE_SIZE, 8,
            EGL_GREEN_SIZE, 8,
            EGL_RED_SIZE, 8,
            EGL_DEPTH_SIZE,24,
            EGL_NONE
        };
        EGLint 	format(0);
        EGLint	numConfigs(0);
        EGLint  major;
        EGLint  minor;

        //! 1
        _display	    =	eglGetDisplay(EGL_DEFAULT_DISPLAY);

        //! 2init
        eglInitialize(_display, &major, &minor);

        //! 3
        eglChooseConfig(_display, attribs, &_config, 1, &numConfigs);

        eglGetConfigAttrib(_display, _config, EGL_NATIVE_VISUAL_ID, &format);
        //! 4 
        _surface	    = 	eglCreateWindowSurface(_display, _config, _hWnd, NULL);

        //! 5
        EGLint attr[]   =   { EGL_CONTEXT_CLIENT_VERSION, 2, EGL_NONE, EGL_NONE };
        _context 	    = 	eglCreateContext(_display, _config, 0, attr);
        //! 6
        if (eglMakeCurrent(_display, _surface, _surface, _context) == EGL_FALSE)
        {
            return false;
        }

        eglQuerySurface(_display, _surface, EGL_WIDTH,  &_winWidth);
        eglQuerySurface(_display, _surface, EGL_HEIGHT, &_winHeight);

        //! windows api
        SendMessage(_hWnd,WM_SIZE,0,0);
        return  true;
    }
    /**
    *   �ر�
    */
    void    shutDownDevice()
    {
       
         onDestroy();
         if (_display != EGL_NO_DISPLAY)
         {
             eglMakeCurrent(_display, EGL_NO_SURFACE, EGL_NO_SURFACE, EGL_NO_CONTEXT);
             if (_context != EGL_NO_CONTEXT) 
             {
                 eglDestroyContext(_display, _context);
             }
             if (_surface != EGL_NO_SURFACE) 
             {
                 eglDestroySurface(_display, _surface);
             }
             eglTerminate(_display);
         }
         _display    =   EGL_NO_DISPLAY;
         _context    =   EGL_NO_CONTEXT;
         _surface    =   EGL_NO_SURFACE;

        UnregisterClass( _T("CELLWinApp"), _hInstance );
    }
    /**
    *   �¼�
    */
    virtual int     events(unsigned msg, unsigned wParam, unsigned lParam)
    {
    #ifndef GET_X_LPARAM
        #define GET_X_LPARAM(lp)                        ((int)(short)LOWORD(lp))
    #endif

    #ifndef GET_Y_LPARAM
        #define GET_Y_LPARAM(lp)                        ((int)(short)HIWORD(lp))
    #endif

    #ifndef GET_WHEEL_DELTA_WPARAM
        #define GET_WHEEL_DELTA_WPARAM(wParam)          (int)((short)HIWORD(wParam))
    #endif

        switch( msg )
        {
        case WM_SIZE:
            {
                RECT    rt;
                GetClientRect(_hWnd,&rt);
                _winWidth   =   rt.right - rt.left;
                _winHeight  =   rt.bottom - rt.top;
            }
            break;
        case WM_LBUTTONDOWN:
            {
            }
            break;
        case WM_LBUTTONUP:
            {
            }
            break;
        case WM_RBUTTONDOWN:
            {
            }
            break;
        case WM_RBUTTONUP:
            {
            }
            break;
        case WM_MOUSEMOVE:
            {
            }
            break;

        case WM_MOUSEWHEEL:
            {
            }
            break;
        case WM_CHAR:
            {
            }
            break;
        case WM_KEYDOWN:
            {
            }
            break;
        case WM_CLOSE:
        case WM_DESTROY:
            {
                ::PostQuitMessage(0);
            }
            break;
        default:
            return DefWindowProc(_hWnd, msg, wParam, lParam );
        }
        return  0;
    }
public:
    /**
    *   ����һ����ʼ��OpenGL�ĺ���,�ڶ���������
    *   ���øú�����ɶ�OpenGL�Ļ���״̬�ĳ�ʼ��
    *   �ڽ�����Ϣѭ��֮ǰ��һ��֪ͨ,ֻ����һ��
    */
    virtual void    onInit()
    {
        /**
        *   ��մ���Ϊ��ɫ
        */
        glClearColor(0,0,0,1);
        /**
        *   ����OpenGL�ӿڵ�λ�úʹ�С��
        */
        glViewport( 0, 0, (GLint) _winWidth, (GLint) _winHeight );
    }
    virtual void        onDestroy()
    {
    }
protected:
    /**
    *   �������ں���
    */
    bool    _createWindow(int width,int height)
    {
        _hWnd   =   CreateWindowEx(
                                    NULL,
                                    _T("CELLWinApp"),
                                    _T("CELLWinApp"),
                                    WS_OVERLAPPEDWINDOW,
                                    CW_USEDEFAULT,
                                    CW_USEDEFAULT,
                                    width,
                                    height, 
                                    NULL, 
                                    NULL,
                                    _hInstance, 
                                    this    //! ����ע�⣬����ǰ���ָ����Ϊ����������,�μ� windowProc����.
                                    );
       
        if( _hWnd == 0 )
        {
            return  false;
        }
        ShowWindow( _hWnd, SW_SHOW );
        UpdateWindow( _hWnd );
        return  true;
    }
    /**
    *   Windows��Ϣ���̴�����
    */
    static  LRESULT CALLBACK    windowProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
    {
#define GWL_USERDATA (-21)
        /**
        *   ʹ��this���ݣ���ȫ�ֺ�����ת��Ϊ��ĳ�Ա��������
        */
        CELLWinApp*  pThis   =   (CELLWinApp*)GetWindowLong(hWnd,GWL_USERDATA);
        if (pThis)
        {
            return  pThis->events(msg,wParam,lParam);
        }
        if (WM_CREATE == msg)
        {
            CREATESTRUCT*   pCreate =   (CREATESTRUCT*)lParam;
            SetWindowLong(hWnd,GWL_USERDATA,(DWORD_PTR)pCreate->lpCreateParams);
        }
        return  DefWindowProc( hWnd, msg, wParam, lParam );
    }
};