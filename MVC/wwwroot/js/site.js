// Stocka — application shell behavior.
// Handles: expandable nav groups, collapsed rail + flyout (desktop),
// off-canvas sidebar (mobile), and persistence of user preferences.

(function () {
    'use strict';

    var app = document.getElementById('app');
    var sidebar = document.getElementById('appSidebar');
    var toggle = document.getElementById('sidebarToggle');
    var overlay = document.getElementById('appOverlay');

    if (!app || !sidebar || !toggle) {
        return;
    }

    var desktopMq = window.matchMedia('(min-width: 992px)');
    var STORAGE_SIDEBAR = 'stocka.sidebar';
    var STORAGE_GROUPS = 'stocka.navGroups';

    var groupButtons = Array.prototype.slice.call(sidebar.querySelectorAll('[data-nav-group]'));
    var flyout = sidebar.querySelector('#navFlyout');
    var flyoutHeader = sidebar.querySelector('#navFlyoutHeader');
    var flyoutList = sidebar.querySelector('#navFlyoutList');
    var activeFlyoutBtn = null;

    function isDesktop() {
        return desktopMq.matches;
    }

    // ---- Collapsible nav groups (expanded mode) ----------------------------

    function applyGroupState(btn, open, persist) {
        var li = btn.closest('.nav-tree__group');
        if (!li) {
            return;
        }
        li.classList.toggle('is-open', open);
        btn.setAttribute('aria-expanded', open ? 'true' : 'false');
        if (persist) {
            saveOpenGroups();
        }
    }

    function openGroupIds() {
        return groupButtons
            .filter(function (btn) { return btn.closest('.nav-tree__group').classList.contains('is-open'); })
            .map(function (btn) { return btn.getAttribute('aria-controls'); });
    }

    function saveOpenGroups() {
        try {
            localStorage.setItem(STORAGE_GROUPS, JSON.stringify(openGroupIds()));
        } catch (e) { /* storage unavailable */ }
    }

    function restoreOpenGroups() {
        var raw = null;
        try {
            raw = localStorage.getItem(STORAGE_GROUPS);
        } catch (e) { /* ignore */ }
        if (raw === null) {
            return; // no saved preference — keep Razor defaults (active group stays open)
        }
        var saved = [];
        try {
            saved = JSON.parse(raw) || [];
        } catch (e) { /* ignore */ }
        groupButtons.forEach(function (btn) {
            var id = btn.getAttribute('aria-controls');
            applyGroupState(btn, saved.indexOf(id) !== -1, false);
        });
        saveOpenGroups();
    }

    // ---- Collapsed rail flyout (desktop) -----------------------------------

    function closeFlyout() {
        if (flyout && activeFlyoutBtn) {
            activeFlyoutBtn.setAttribute('aria-expanded', 'false');
        }
        activeFlyoutBtn = null;
        if (flyout) {
            flyout.classList.remove('is-open');
        }
    }

    function openFlyout(btn) {
        var li = btn.closest('.nav-tree__group');
        var sub = li && li.querySelector('.nav-sub');
        if (!flyout || !sub) {
            return;
        }

        // Repopulate from the group's inline sub-menu so the flyout always
        // mirrors the source of truth (labels, hrefs, active state).
        var label = btn.querySelector('.nav-link__label');
        if (flyoutHeader) {
            flyoutHeader.textContent = label ? label.textContent.trim() : '';
        }
        if (flyoutList) {
            flyoutList.replaceChildren();
            sub.querySelectorAll('a.nav-link--sub').forEach(function (link) {
                flyoutList.appendChild(link.cloneNode(true));
            });
        }

        // Position beside the rail. Logical, direction-aware placement.
        var rect = btn.getBoundingClientRect();
        var gap = 8;
        var isRtl = getComputedStyle(document.documentElement).direction === 'rtl';
        flyout.style.top = rect.top + 'px';
        flyout.style.left = 'auto';
        flyout.style.right = 'auto';
        if (isRtl) {
            flyout.style.right = Math.max(8, window.innerWidth - rect.left + gap) + 'px';
        } else {
            flyout.style.left = rect.right + gap + 'px';
        }

        // Clamp so the panel never leaves the viewport vertically.
        var maxTop = window.innerHeight - flyout.offsetHeight - 8;
        if (maxTop < 8) {
            maxTop = 8;
        }
        flyout.style.top = Math.min(rect.top, maxTop) + 'px';

        activeFlyoutBtn = btn;
        btn.setAttribute('aria-expanded', 'true');
        flyout.classList.add('is-open');
    }

    // ---- Collapsed / expanded mode (desktop) --------------------------------

    function applySidebarState(collapsed) {
        sidebar.classList.toggle('is-collapsed', collapsed);
        toggle.setAttribute('aria-expanded', String(!collapsed));
        toggle.setAttribute('aria-label', collapsed ? 'Expand sidebar' : 'Collapse sidebar');
        closeFlyout();
        try {
            localStorage.setItem(STORAGE_SIDEBAR, collapsed ? 'collapsed' : 'expanded');
        } catch (e) { /* ignore */ }
    }

    function restoreSidebarState() {
        if (!isDesktop()) {
            return;
        }
        var saved = null;
        try {
            saved = localStorage.getItem(STORAGE_SIDEBAR);
        } catch (e) { /* ignore */ }
        if (saved === 'collapsed') {
            applySidebarState(true);
        }
    }

    // ---- Off-canvas sidebar (mobile) ----------------------------------------

    function setMobileOpen(open) {
        app.classList.toggle('is-open', open);
        toggle.setAttribute('aria-expanded', String(open));
        if (open) {
            toggle.setAttribute('aria-label', 'Close navigation');
            // Move focus into the sidebar once the slide-in has started.
            window.setTimeout(function () {
                var first = sidebar.querySelector('a, button');
                if (first) { first.focus(); }
            }, 240);
        } else {
            toggle.setAttribute('aria-label', 'Open navigation');
            toggle.focus();
        }
    }

    // ---- Events --------------------------------------------------------------

    toggle.addEventListener('click', function () {
        if (isDesktop()) {
            applySidebarState(!sidebar.classList.contains('is-collapsed'));
        } else {
            setMobileOpen(!app.classList.contains('is-open'));
        }
    });

    if (overlay) {
        overlay.addEventListener('click', function () {
            if (!isDesktop()) {
                setMobileOpen(false);
            }
        });
    }

    // Group buttons: expand inline in expanded mode, open the flyout when the
    // sidebar is collapsed.
    groupButtons.forEach(function (btn) {
        btn.addEventListener('click', function (event) {
            event.stopPropagation();
            if (sidebar.classList.contains('is-collapsed') && isDesktop()) {
                if (activeFlyoutBtn === btn) {
                    closeFlyout();
                } else {
                    openFlyout(btn);
                }
                return;
            }
            var li = btn.closest('.nav-tree__group');
            applyGroupState(btn, !li.classList.contains('is-open'), true);
        });

        // Hover opens the flyout on the rail (pointer devices).
        btn.addEventListener('mouseenter', function () {
            if (sidebar.classList.contains('is-collapsed') && isDesktop()) {
                openFlyout(btn);
            }
        });
    });

    document.addEventListener('pointerdown', function (event) {
        if (!isDesktop()) {
            return;
        }
        if (flyout && !flyout.contains(event.target) && !(event.target.closest && event.target.closest('[data-nav-group]'))) {
            closeFlyout();
        }
    });

    document.addEventListener('scroll', closeFlyout, true);
    window.addEventListener('resize', function () {
        if (isDesktop() && activeFlyoutBtn) {
            openFlyout(activeFlyoutBtn);
        }
    });

    document.addEventListener('keydown', function (event) {
        if (event.key !== 'Escape') {
            return;
        }
        if (!isDesktop() && app.classList.contains('is-open')) {
            setMobileOpen(false);
            return;
        }
        if (activeFlyoutBtn) {
            closeFlyout();
            activeFlyoutBtn.focus();
        }
    });

    // Keep state consistent when crossing the desktop/mobile boundary.
    desktopMq.addEventListener('change', function () {
        closeFlyout();
        if (isDesktop()) {
            setMobileOpen(false);
            restoreSidebarState();
        } else {
            sidebar.classList.remove('is-collapsed');
            toggle.setAttribute('aria-expanded', 'false');
        }
    });

    // ---- Boot ------------------------------------------------------------------

    restoreSidebarState();
    restoreOpenGroups();
})();
