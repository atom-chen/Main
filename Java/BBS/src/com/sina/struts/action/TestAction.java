/*
 * Generated by MyEclipse Struts
 * Template path: templates/java/JavaClass.vtl
 */
package com.sina.struts.action;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.apache.struts.action.ActionForm;
import org.apache.struts.action.ActionForward;
import org.apache.struts.action.ActionMapping;
import org.apache.struts.actions.DispatchAction;

import com.sina.domain.Type;
import com.sina.service.inter.TestInter;

/** 
 * MyEclipse Struts
 * Creation date: 06-16-2011
 * 
 * XDoclet definition:
 * @struts.action parameter="flag"
 */
public class TestAction extends DispatchAction {
	/*
	 * Generated Methods
	 */

	
	private TestInter testService2;
	
	public ActionForward test(ActionMapping mapping, ActionForm form,
			HttpServletRequest request, HttpServletResponse response) {
		// TODO Auto-generated method stub
		//这里测试一下，看看能不能添加一个Type进去
		Type type=new Type();
		type.setType("php技术");
		
		testService2.save(type);
		
		return mapping.findForward("ok");
	}

	public void setTestService2(TestInter testService2) {
		this.testService2 = testService2;
	}
}