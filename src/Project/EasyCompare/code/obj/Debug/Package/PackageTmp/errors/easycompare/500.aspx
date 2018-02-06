<!DOCTYPE html>
<html>
<head>
    <link rel="stylesheet" type="text/css" href="/assets/easycompare/css/swiper.min.css">
    <link href="/assets/easycompare/css/main.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="/assets/easycompare/css/pages.css">
     <link rel="stylesheet" type="text/css" href="/assets/easycompare/css/fonts.css">
    <title>EasyCompare</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script type="text/javascript" src="https://ajax.microsoft.com/ajax/jQuery/jquery-1.4.2.min.js"></script>
</head>
<body>
    <a name="top">
        <div class="lightbox">
            <div class="lightbox-content">
                <span class="lightboxclose cursor" onclick="">&times;</span>
                <div id="lightbox_title" class="lightbox-title">
                    <h2>Lightbox Title</h2>
                </div>
                <div id="lightbox_contents" class="lightbox-inner-content">Inner Content</div>

            </div>
        </div>



        <%--<div id="lb_search" class="lightbox">
            <div class="lightbox-content">
                <span class="lightboxclose cursor" onclick="closebox('lb_search');">&times;</span>
                <div id="lightbox_title" class="lightbox-title">
                    <h2>Search The Site</h2>
                </div>
                <div id="lightbox_contents" class="lightbox-inner-content">

                    <div class="lb-form">
                        <div class="input-container">
                            <input placeholder="Type here" class="form-input-search" type="text" />
                        </div>
                        <div class="input-container">
                            <button class="button-search">Search</button>
                        </div>
                    </div>

                </div>

            </div>
        </div>--%>
        <div class="header_bottom_right_icon">
        
<img onclick="show('lb_search')" width="25" src="/assets/easycompare/img/msearch_w.png">

<div id="lb_search" class="lightbox" style="display: block;">
    <div class="lightbox-content">
        <span class="lightboxclose cursor" onclick="closebox('lb_search');">×</span>
        <div id="lightbox_title" class="lightbox-title"> <h2>Search The Site</h2></div>
        <div id="lightbox_contents" class="lightbox-inner-content">
            <div class="lb-form">
                <div class="search_input-container">
                    <input placeholder="Type here" id="searchTerm" class="form-input-search" type="text">
                </div>
                <div class="search_input-container">
                    <button class="button-search" data-searchresultspage="/search">Search</button>
                </div>
            </div>
        </div>
    </div>
</div>

    </div>


        <header class="centered-content">
            <div class="header_top">

                <div class="header_top_left_icon">
                    <img width="150" src="/assets/easycompare/img/logo/easycompare-logo.png" onclick="window.location.href='/'">
                </div>

                <div class="header_top_right">
                    <a class="phonelink" href="tel:5551234567">02 222 6555 </a>
                </div>
                <div class="header_top_right_icon">
                    <a class="phonelink" href="tel:5551234567">
                        <img width="25" src="/assets/easycompare/img/phone-gray.png">
                    </a>
                </div>

            </div>
            <div class="header_bottom">


                <div class="header_bottom_left">
                    <span class="header_menu_item" onclick="window.location.href='/product-and-services'" title="">Products And Services
                    </span>
                    <span class="header_menu_item" onclick="window.location.href='/insurance-talk'" title="">Blog
                    </span>
                    <span class="header_menu_item" onclick="window.location.href='/about-us'" title="">About Us
                    </span>
                    <span class="header_menu_item" onclick="window.location.href='/support'" title="">Customer Support
                    </span>
                </div>

                <div class="header_bottom_right_icon">
                    <img onclick="show('lb_search')" width="25" src="/assets/easycompare/img/msearch_w.png">
                </div>
              <%--  <div class="header_bottom_right">
                    <span class="langactive">EN </span>| TH     
                </div>--%>
                <div class="header_bottom_right">
                    <span onclick="window.location.href='/en'" class="">EN </span>|
                    <span class="langactive"><a href="/th-th">TH</a></span>
                </div>


                <div id="menuBtn" class="header_bottom_left_mobile">
                    <img width="25" src="/assets/easycompare/img/hamburger-menu.png">
                </div>


                <div id="widgetMenu" class="hidden widgetsmenu">
                    <ul class="twopanel-menu right-panel">
                        <li class="menu-header" onclick="window.location.href='http://www.google.com'">Products &amp; Services
                        </li>
                        <li class="menu-header" onclick="window.location.href='/insurance-talk'">Blog
                        </li>
                        <li class="menu-header" onclick="window.location.href='/about-us'">About Us
                        </li>
                        <li class="menu-header" onclick="window.location.href='/support'">Support
                        </li>

                    </ul>
                </div>



            </div>

        </header>

        <main class="centered-content">


        <div class="header-navigation">
            <span class="nav-heading ">500 - Error </span>
        </div>


        <div class="main-container">


            <span class="article__title">Well, I guess we are lost..</span><br />
            <div class="article__summary">You can try the links above or use the search below or let us call you.</div>
            <div class="article__image" style="background-image: url('/assets/easycompare/img/home/articles/1.png')"></div>

            <div class="p404-form">
                <div class="input-container">
                    <input placeholder="Type here to search Easy Compare" class="form-input-search" type="text" />
                </div>
                <div class="input-container">
                    <button class="button-search">Search</button>
                </div>
            </div>




        </div>
    </div>
    </main>

    <%--    <div class="footer__links centered-content">
            <div class="footer__links__main__menu grid">
                <div class="footer__links__main__menu__item grid-item">
                </div>
            </div>
            <div class="footer__links__sub__menu grid">
                <div class="footer__links__sub__menu__item grid-item">
                    <div class="footer__col__item logo">
                        <img src='/-/media/easycompare/images/logo/footerec-logo.png?h=64&amp;w=210&amp;la=th-TH&amp;hash=560695477C9DFDBF6D2272D9AEE25831B4CBCEDD' class='whitelogo' alt='' />
                    </div>

                    <div class="footer__col__item copyright">
                        © 2017 Easy Compare
                    </div>
                    <div class="footer__col__item links">
                        <span class="links-separated"><a href="generic.html">Privacy & Policy</a></span>
                        <span class="links-separated"><a class="links-separated" href="generic.html">Term of use</a></span>
                    </div>
                    <div class="footer__col__item logos">
                        <img src="" />
                        <div class="footer_logo_container">
                            <div class="footer_logo" style="background-image: url('/-/media/easycompare/images/partners-links/u309.png'); background-size: 100%;"></div>
                        </div>
                        <div class="footer_logo_container">
                            <div class="footer_logo" style="background-image: url('/-/media/easycompare/images/partners-links/u309.png'); background-size: 100%;"></div>
                        </div>
                        <div class="footer_logo_container">
                            <div class="footer_logo" style="background-image: url('/-/media/easycompare/images/partners-links/u309.png'); background-size: 100%;"></div>
                        </div>
                    </div>
                </div>

                <div class="footer__links__sub__menu__item grid-item show-for-medium">
                    <ul>
                        <li class="footer__links__sub__menu__item__header">About Us</li>
                        <li>
                            <a href="/career">Career</a>
                        </li>
                        <li>
                            <a href="/contact-info">Contact Info</a>
                        </li>
                        <li>
                            <a href="/corporate-info">Corporate Info</a>
                        </li>
                        <li>
                            <a href="/mission-and-vision">Mission and Vision</a>
                        </li>
                    </ul>
                </div>
                <div class="footer__links__sub__menu__item grid-item show-for-medium">
                    <ul>
                        <li class="footer__links__sub__menu__item__header">Support</li>
                        <li>
                            <a href="/ask-a-question">Ask a Question</a>
                        </li>
                        <li>
                            <a href="/call-me-back">Call Me Back</a>
                        </li>
                        <li>
                            <a href="/faq">FAQ</a>
                        </li>
                    </ul>
                </div>

                <div class="footer__links__sub__menu__item grid-item">
                    <div class="footer__col__item bold spacetobottom">
                        <a class="phonelink_footer  fphone" href="tel:02 222 6555">02 222 6555</a>
                    </div>
                    <div class="footer__col__item bold spacetobottom">
                        <a class="phonelink_footer fclock">Mon-Fri, 8am-8pm</a>
                    </div>
                    <div class="footer__col__item bold whitetext ">
                        <a class="phonelink_footer  femail" href="mailto:info@easycompare.co.th">info@easycompare.co.th</a>
                    </div>
                    <div class="footer__col__item bold margintop whitetext">
                        Follow us on
                    </div>
                    <div class="footer__col__item social-icons">
                        <span>
                            <a href='http://www.facebook.com'>
                                <img src="/-/media/easycompare/images/footer-social/facebook-icon.png" />
                            </a></span>
                        <span>
                            <a href='http://www.twitter.com'>
                                <img src="/-/media/easycompare/images/footer-social/twitter-icon.png" />
                            </a></span>
                        <span>
                            <a href='http://www.youtube.com'>
                                <img src="/-/media/easycompare/images/footer-social/youtube-icon.png" />
                            </a></span>
                        <span>
                            <a href='http://www.line.com'>
                                <img src="/-/media/easycompare/images/footer-social/line-icon.png" />
                            </a></span>
                    </div>
                </div>

            </div>
        </div>--%>
        <div class="footer__links centered-content">
        <div class="footer__links__sub__menu">
            <div class="footer__links__sub__menu__item foot-menu-item-head">
                    <div class="footer__col__item logo">
                        <img src="/-/media/easycompare/images/logo/footerec-logo.png?h=64&amp;w=210&amp;la=en&amp;hash=47C780A67EF6B5C301E7189C8303B2D27ED3A5BD" class="whitelogo" alt="">
                    </div>

<style type="text/css">
.address-footer {color: #939393;margin-bottom:5px;}
</style>
<div class="address-footer">Easy Compare (Thailand) Co., Ltd<br>57 Soi Pipat Silom, Bang Rak <br>Bangkok 10500</div>
<div class="footer__col__item copyright">
© 2017 Easy Compare
</div>
<div class="footer__col__item links">
<span class="links-separated"><a href="/privacy-and-policy">Privacy &amp; Policy</a></span>
<span class="links-separated"><a href="/terms-of-use">Term of use</a></span>
</div>                <div class="footer__col__item logos">
                                    <img src="">
                                    <img src="">
                                    <img src="">
                                    <img src="">
                </div>
            </div>

                <div class="footer__links__sub__menu__item foot-menu-item show-for-medium">
                    <ul>
                        <li class="footer__links__sub__menu__item__header">Customer Support</li>
                            <li>
                                    <a href="/car-insurance">Car Insurance</a>
                            </li>
                            <li>
                                    <a href="/our-partners">Our Partners</a>
                            </li>
                            <li>
                                    <a href="/product-and-services">Our Service</a>
                            </li>
                    </ul>
                </div>
                <div class="footer__links__sub__menu__item foot-menu-item show-for-medium">
                    <ul>
                        <li class="footer__links__sub__menu__item__header">Blog</li>
                            <li>
                                    <a href="http://10.34.28.119/insurance-talk?tagname={CEF46E84-03F3-498F-8E3E-0BEDA68E3D12}">Promotion</a>
                            </li>
                            <li>
                                    <a href=""></a>
                            </li>
                            <li>
                                    <a href="/insurance-talk?tagname={76494E61-4E7A-4540-9BD2-B33067FFB6FF}">Tips and Insights</a>
                            </li>
                    </ul>
                </div>
                <div class="footer__links__sub__menu__item foot-menu-item show-for-medium">
                    <ul>
                        <li class="footer__links__sub__menu__item__header">About Us</li>
                            <li>
                                    <a href="/career">Career</a>
                            </li>
                            <li>
                                    <a href="/customer-support">Contact Info</a>
                            </li>
                            <li>
                                    <a href="/about-us">Corporate Info</a>
                            </li>
                            <li>
                                    <a href="/vision">Vision</a>
                            </li>
                    </ul>
                </div>
                <div class="footer__links__sub__menu__item foot-menu-item show-for-medium">
                    <ul>
                        <li class="footer__links__sub__menu__item__header">Customer Support</li>
                            <li>
                                    <a href="/ask-a-question">Ask A Question</a>
                            </li>
                            <li>
                                    <a href="/call-me-back">Call Me Back</a>
                            </li>
                            <li>
                                    <a href="/faq">FAQ</a>
                            </li>
                    </ul>
                </div>

            <div class="footer__links__sub__menu__item foot-menu-item-end show-for-small-up">
                <div class="footer__col__item bold spacetobottom">
                    <a class="phonelink_footer  fphone" href="tel:02 206 8555">02 206 8555</a>
                </div>
                <div class="footer__col__item bold spacetobottom">
                    <a class="phonelink_footer fclock">Mon-Fri, 8.30 am - 7.00 pm</a>
                </div>
                <div class="footer__col__item bold whitetext ">
                    <a class="phonelink_footer  femail" href="mailto:sawasdee@easycompare.co.th">sawasdee@easycompare.co.th</a>
                </div>
                <div class="footer__col__item bold margintop whitetext">
                    Follow us on
                </div>
                <div class="footer__col__item social-icons">
                    <span>
<a href="http://www.facebook.com">                                    <img src="/-/media/easycompare/images/footer-social/facebook-icon.png">
</a>                    </span>
                    <span>
<a href="http://www,twitter.com">                                    <img src="/-/media/easycompare/images/footer-social/twitter-icon.png">
</a>                    </span>
                    <span>
<a href="http://www,youtube.com">                                    <img src="/-/media/easycompare/images/footer-social/youtube-icon.png">
</a>                    </span>
                    <span>
                                    <img src="/-/media/easycompare/images/footer-social/line-icon.png">
                    </span>
                </div>
            </div>

        </div>
    </div>



       <%-- <div id="bottom-contact-bar" class="bottom-contact-bar">
            <div class="help-you-txt">Need help? Call us at 02-222 6555 or Let us call you!</div>

            <div class="cb_ncontainer">
                <div class="help-you-title">How to address you?</div>
                <div class="contact-item name-container">
                    <input class="name-input" placeholder="enter name" />
                </div>
            </div>

            <div class="cb_ncontainer">
                <div class="help-you-title">How to reach you?</div>
                <div class="name-container">
                    <span id="emailbutton" onclick="contactmewith('email')" class="emailbutton filter_on"></span>
                    <span id="phonebutton" onclick="contactmewith('phone')" class="phonebutton"></span>
                    <span id="linebutton" onclick="contactmewith('line')" class="linebutton"></span>
                    <input id="contact_method" class="contact-input" placeholder="enter email" />
                </div>
            </div>

            <div class="cb_ncontainer">
                <div class="help-you-title">When we can reach you?</div>
                <div class="contact-item name-container">
                    <select class="name-input time-dropdown">
                        <option>Morning (8:30am - Midday)</option>
                        <option>Afternoon (Midday - 5pm)</option>
                        <option>Evening (5pm - 7pm)</option>
                    </select>
                    <!--  <div class="dropdown-arrow"></div> -->
                </div>
            </div>

            <div class="cb_bcontainer">
                <span onclick="window.location.href='thanks.html'" class="contact-btn name-label">Call me back!</span>
                <div class="cb_bfiller">&nbsp</div>
            </div>

        </div>--%>
</body>
<script type="text/javascript">



    function show(box) {
        document.getElementById(box).style.display = "block";
        document.getElementsByTagName("body")[0].style.overflow = "hidden";

    }

    function closebox(box) {
        document.getElementById(box).style.display = "none";
        document.getElementsByTagName("body")[0].style.overflow = "auto";
    }


</script>
<script src="/assets/easycompare/js/main-.js"></script>
</html>
