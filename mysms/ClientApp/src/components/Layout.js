import React, { Component } from 'react';
import { NavLink, Link } from 'react-router-dom';
import { Container } from 'reactstrap';
import DashBoard from './DashBoard';
import { NavMenu } from './NavMenu';
import "./Layout.css";
export class Layout extends Component {
  static displayName = Layout.name;

  render () {
      return (
          <>
              <div className="row">
                  <div className="col-md-2">
                      <div className="sidebar px-4 py-4 py-md-4 me-0">
                          <div className="d-flex flex-column h-100">
                              <a href="index.html" className="mb-0 brand-icon">
                                  <span className="logo-icon">
                                      <i className="bi bi-bag-check-fill fs-4"></i>
                                  </span>
                                  {/*<span class="logo-text">eBazar</span>*/}
                                  <span className="logo-text"><NavLink tag={Link} className="text-dark" to="/">DashBoard</NavLink></span>
                              </a>

                              <ul className="menu-list flex-grow-1 mt-3">
                                  <li>
                                      <i className="icofont-focus fs-5"></i>  <span><NavLink tag={Link} className="text-dark" to="/smsInbox">SmsInbox</NavLink> </span> 
                                  </li>
                                 
                                  <li className="collapsed">
                                      <a className="m-link" data-bs-toggle="collapse" data-bs-target="#send-sms" href="#">
                                          <i className="icofont-truck-loaded fs-5"></i> <span>Send Sms </span> <span class="arrow icofont-rounded-down ms-auto text-end fs-5"></span>
                                      </a>

                                      <ul className="sub-menu collapse" id="send-sms">
                                          <li>
                                              <i className="icofont-focus fs-5"></i>  <span><NavLink tag={Link} className="text-dark" to="/sendSms">Send Sms</NavLink> </span>
                                             
                                          </li>
                                          <li>
                                              <i className="icofont-focus fs-5"></i>  <span><NavLink tag={Link} className="text-dark" to="/sendSmsList">Send Sms List</NavLink> </span>
                                          </li>

                                      </ul>
                                  </li>

                                  <li className="collapsed">
                                      <a className="m-link" data-bs-toggle="collapse" data-bs-target="#menu-product" href="#">
                                          <i className="icofont-truck-loaded fs-5"></i> <span>Contact List</span> <span class="arrow icofont-rounded-down ms-auto text-end fs-5"></span>
                                      </a>

                                      <ul className="sub-menu collapse" id="menu-product">



                                          <li><NavLink tag={Link} className="text-dark" to="/contactAdd">Contact Add</NavLink> </li>
                                          <li><NavLink tag={Link} className="text-dark" to="/contactList">Contact List</NavLink> </li>

                                      </ul>
                                  </li>
                                  <li className="collapsed">
                                      <a className="m-link" data-bs-toggle="collapse" data-bs-target="#dailyReport" href="#">
                                          <i className="icofont-truck-loaded fs-5"></i> <span>Daily Report</span> <span class="arrow icofont-rounded-down ms-auto text-end fs-5"></span>
                                      </a>

                                      <ul className="sub-menu collapse" id="dailyReport">
                                          <li><NavLink tag={Link} className="text-dark" to="/dailyReportOpening">Daily Report Opening</NavLink> </li>
                                          <li><NavLink tag={Link} className="text-dark" to="/dailyReportClosing">Daily Report Closing</NavLink> </li>

                                      </ul>
                                  </li>

                                  <li>
                                      <i className="icofont-focus fs-5"></i>  <span><NavLink tag={Link} className="text-dark" to="/ussdAdd">Ussd Send</NavLink> </span>
                                  </li>
                                  <li>
                                      <i className="icofont-focus fs-5"></i>  <span><NavLink tag={Link} className="text-dark" to="/portSetting">Port Setting</NavLink> </span>
                                  </li>
                                  <li className="collapsed">
                                      <a className="m-link" data-bs-toggle="collapse" data-bs-target="#group" href="#">
                                          <i className="icofont-truck-loaded fs-5"></i> <span>Group</span> <span class="arrow icofont-rounded-down ms-auto text-end fs-5"></span>
                                      </a>

                                      <ul className="sub-menu collapse" id="group">
                                          <li><NavLink tag={Link} className="text-dark" to="/groupList">Group List</NavLink> </li>
                                          <li><NavLink tag={Link} className="text-dark" to="/groupAdd">Group Add</NavLink> </li>

                                      </ul>
                                  </li>
                                  <li>
                                      <i className="icofont-focus fs-5"></i>  <span><NavLink tag={Link} className="text-dark" to="/groupWiseSms">Group Wise Sms</NavLink> </span>
                                  </li>
                              </ul>

                              <button type="button" className="btn btn-link sidebar-mini-btn text-light">
                                  <span className="ms-2"><i className="icofont-bubble-right"></i></span>
                              </button>
                          </div>
                      </div>

                  </div>
                  <div className="col-md-10 component-part">
                      {this.props.children}

                  </div>
           
              </div>
             
              </>
    );
  }
}
