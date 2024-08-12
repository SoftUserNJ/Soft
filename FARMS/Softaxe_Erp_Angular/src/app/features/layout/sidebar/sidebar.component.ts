import { Component, AfterViewInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { SharedService } from 'src/app/services/shared.service';
declare var $: any;

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements AfterViewInit {
  isApproval = localStorage.getItem('approvalSystem');
  isDayClose = localStorage.getItem('dayClose');
  superAdmin = localStorage.getItem('superAdmin');
  admin = localStorage.getItem('admin');
  designation = localStorage.getItem('designation');
  costCenter = localStorage.getItem('costCenter');
  mobApp = localStorage.getItem('mobApp');
  locId = localStorage.getItem('locId');
  cmpId = localStorage.getItem('cmpId');
  distributionPos = localStorage.getItem('distributionPos');

  constructor(
    public sharedService: SharedService,
    private apiService: ApiService
  ) {}

  async ngOnInit() {
    await this.allowMenu();
  }

  ngAfterViewInit() {
    $(document).ready(function () {
      const toggleBtn = $('#toggle_btn');
      const body = $('body');
      const headerLeft = $('.header-left');
      const $slimScrolls = $('.slimscroll');

      toggleBtn.on('click', function () {
        if (body.hasClass('mini-sidebar')) {
          body.removeClass('mini-sidebar');
          $(this).addClass('active');
          localStorage.setItem('screenModeNightTokenState', 'night');
          setTimeout(function () {
            body.removeClass('mini-sidebar');
            headerLeft.addClass('active');
          }, 100);
        } else {
          body.addClass('mini-sidebar');
          $(this).removeClass('active');
          localStorage.removeItem('screenModeNightTokenState');
          setTimeout(function () {
            body.addClass('mini-sidebar');
            headerLeft.removeClass('active');
          }, 100);
        }
        return false;
      });

      $(document).on('mouseover', function (e) {
        e.stopPropagation();
        if (
          $('body').hasClass('mini-sidebar') &&
          $('#toggle_btn').is(':visible')
        ) {
          var targ = $(e.target).closest('.sidebar, .header-left').length;
          if (targ) {
            $('body').addClass('expand-menu');
            $('.subdrop + ul').slideDown();
          } else {
            $('body').removeClass('expand-menu');
            $('.subdrop + ul').slideUp();
          }
          return false;
        }
        return false; // Add this line to handle other cases
      });

      init();

      function init() {
        $('#sidebar-menu a').on('click', function (e) {
          if ($(this).parent().hasClass('submenu')) {
            e.preventDefault();
          }
          if (!$(this).hasClass('subdrop')) {
            $('ul', $(this).parents('ul:first')).slideUp(250);
            $('a', $(this).parents('ul:first')).removeClass('subdrop');
            $(this).next('ul').slideDown(350);
            $(this).addClass('subdrop');
          } else if ($(this).hasClass('subdrop')) {
            $(this).removeClass('subdrop');
            $(this).next('ul').slideUp(350);
          }
        });
        $('#sidebar-menu ul li.submenu a.active')
          .parents('li:last')
          .children('a:first')
          .addClass('active')
          .trigger('click');
      }

      $('.submenus').on('click', function () {
        $('body').addClass('sidebarrightmenu');
      });

      var $wrapper = $('.main-wrapper');

      $(document).on('click', '#mobile_btn', function () {
        $wrapper.toggleClass('slide-nav');
        $('.sidebar-overlay').toggleClass('opened');
        $('html').addClass('menu-opened');
        $('#task_window').removeClass('opened');
        return false;
      });

      if ($slimScrolls.length > 0) {
        $slimScrolls.slimScroll({
          height: 'auto',
          width: '100%',
          position: 'right',
          size: '7px',
          color: '#ccc',
          wheelStep: 10,
          touchScrollStep: 100,
        });
        var wHeight = $(window).height() - 60;
        $slimScrolls.height(wHeight);
        $('.sidebar .slimScrollDiv').height(wHeight);
        $(window).resize(function () {
          var rHeight = $(window).height() - 60;
          $slimScrolls.height(rHeight);
          $('.sidebar .slimScrollDiv').height(rHeight);
        });
      }
    });
  }

  async allowMenu() {
    const data = await this.apiService
      .getData('Utilities/GetAllowMenu')
      .toPromise();

    $('.securityLi')
      .find('li')
      .each(function () {
        var showmenu;
        var subMenuId = $(this).attr('id');

        $.each(data, function (i, item) {
          if (subMenuId == item.Menuid) {
            showmenu = true;
          }
        });

        if (showmenu == true) {
          $(this).addClass('d-block');
        } else {
          $(this).addClass('d-none');
        }
      });

    $('.securityLi').each(function () {
      let liLength = $(this).find('ul li').length;
      let hide = $(this).find('ul .d-none').length;

      if (liLength == hide) {
        $(this).hide();
      }
    });
  }
}
