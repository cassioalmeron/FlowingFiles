# HTML/CSS UI Prototype Reference

Complete HTML/CSS templates for rapid UI prototyping with shadcn-inspired styling using Tailwind CSS.

## Table of Contents

1. [Base Template](#base-template)
2. [Dashboard Layout](#dashboard-layout)
3. [Form Pages](#form-pages)
4. [Data Tables](#data-tables)
5. [Modal Dialogs](#modal-dialogs)
6. [Settings Pages](#settings-pages)
7. [Login/Signup Pages](#loginsignup-pages)
8. [Landing Pages](#landing-pages)
9. [Component Library](#component-library)

---

## Base Template

Every prototype starts with this:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50">
    <!-- Content here -->
</body>
</html>
```

---

## Dashboard Layout

Complete dashboard with sidebar, stats cards, and table.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Dashboard Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50">
    <div class="flex h-screen">
        <!-- Sidebar -->
        <aside class="hidden lg:block w-64 bg-white border-r">
            <div class="p-6">
                <h2 class="text-xl font-bold text-slate-900">App Name</h2>
            </div>
            <nav class="px-4 space-y-1">
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-white bg-slate-900 rounded-md">
                    Dashboard
                </a>
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-100 rounded-md">
                    Analytics
                </a>
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-100 rounded-md">
                    Users
                </a>
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-100 rounded-md">
                    Settings
                </a>
            </nav>
        </aside>

        <!-- Main Content -->
        <div class="flex-1 flex flex-col overflow-hidden">
            <!-- Top Navigation -->
            <header class="bg-white border-b">
                <div class="flex items-center justify-between px-6 py-4">
                    <button onclick="toggleMobileMenu()" class="lg:hidden p-2 rounded-md hover:bg-gray-100">
                        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path>
                        </svg>
                    </button>
                    <h1 class="text-2xl font-semibold text-slate-900">Dashboard</h1>
                    <div class="flex items-center gap-4">
                        <button class="p-2 rounded-full hover:bg-gray-100">
                            <svg class="w-6 h-6 text-slate-700" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9"></path>
                            </svg>
                        </button>
                        <div class="w-10 h-10 rounded-full bg-slate-900 flex items-center justify-center text-white font-medium">
                            JD
                        </div>
                    </div>
                </div>
            </header>

            <!-- Page Content -->
            <main class="flex-1 overflow-y-auto p-6">
                <!-- Stats Grid -->
                <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
                    <!-- Stat Card 1 -->
                    <div class="bg-white rounded-lg border border-slate-200 shadow-sm p-6">
                        <p class="text-sm font-medium text-slate-600">Total Revenue</p>
                        <p class="text-2xl font-bold text-slate-900 mt-2">$45,231.89</p>
                        <p class="text-xs text-green-600 mt-2">+20.1% from last month</p>
                    </div>

                    <!-- Stat Card 2 -->
                    <div class="bg-white rounded-lg border border-slate-200 shadow-sm p-6">
                        <p class="text-sm font-medium text-slate-600">Subscriptions</p>
                        <p class="text-2xl font-bold text-slate-900 mt-2">+2,350</p>
                        <p class="text-xs text-green-600 mt-2">+180.1% from last month</p>
                    </div>

                    <!-- Stat Card 3 -->
                    <div class="bg-white rounded-lg border border-slate-200 shadow-sm p-6">
                        <p class="text-sm font-medium text-slate-600">Sales</p>
                        <p class="text-2xl font-bold text-slate-900 mt-2">+12,234</p>
                        <p class="text-xs text-green-600 mt-2">+19% from last month</p>
                    </div>

                    <!-- Stat Card 4 -->
                    <div class="bg-white rounded-lg border border-slate-200 shadow-sm p-6">
                        <p class="text-sm font-medium text-slate-600">Active Now</p>
                        <p class="text-2xl font-bold text-slate-900 mt-2">+573</p>
                        <p class="text-xs text-green-600 mt-2">+201 since last hour</p>
                    </div>
                </div>

                <!-- Recent Activity Table -->
                <div class="bg-white rounded-lg border border-slate-200 shadow-sm">
                    <div class="p-6 border-b border-slate-200">
                        <h2 class="text-lg font-semibold text-slate-900">Recent Activity</h2>
                    </div>
                    <div class="overflow-x-auto">
                        <table class="w-full">
                            <thead class="bg-slate-50 border-b border-slate-200">
                                <tr>
                                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase">Name</th>
                                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase">Status</th>
                                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase">Date</th>
                                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase">Amount</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-slate-200">
                                <tr class="hover:bg-slate-50">
                                    <td class="px-6 py-4 text-sm font-medium text-slate-900">John Doe</td>
                                    <td class="px-6 py-4"><span class="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-semibold text-green-800">Active</span></td>
                                    <td class="px-6 py-4 text-sm text-slate-600">2024-01-15</td>
                                    <td class="px-6 py-4 text-sm font-medium text-slate-900">$1,234.00</td>
                                </tr>
                                <tr class="hover:bg-slate-50">
                                    <td class="px-6 py-4 text-sm font-medium text-slate-900">Jane Smith</td>
                                    <td class="px-6 py-4"><span class="inline-flex items-center rounded-full bg-yellow-100 px-2.5 py-0.5 text-xs font-semibold text-yellow-800">Pending</span></td>
                                    <td class="px-6 py-4 text-sm text-slate-600">2024-01-14</td>
                                    <td class="px-6 py-4 text-sm font-medium text-slate-900">$567.00</td>
                                </tr>
                                <tr class="hover:bg-slate-50">
                                    <td class="px-6 py-4 text-sm font-medium text-slate-900">Bob Johnson</td>
                                    <td class="px-6 py-4"><span class="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-semibold text-green-800">Active</span></td>
                                    <td class="px-6 py-4 text-sm text-slate-600">2024-01-13</td>
                                    <td class="px-6 py-4 text-sm font-medium text-slate-900">$890.00</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </main>
        </div>
    </div>

    <!-- Mobile Menu (hidden by default) -->
    <div id="mobile-menu" class="hidden fixed inset-0 bg-black bg-opacity-50 z-50 lg:hidden">
        <div class="w-64 bg-white h-full">
            <div class="p-6 flex items-center justify-between">
                <h2 class="text-xl font-bold text-slate-900">App Name</h2>
                <button onclick="toggleMobileMenu()" class="p-2 rounded-md hover:bg-gray-100">
                    <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                    </svg>
                </button>
            </div>
            <nav class="px-4 space-y-1">
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-white bg-slate-900 rounded-md">Dashboard</a>
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-100 rounded-md">Analytics</a>
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-100 rounded-md">Users</a>
                <a href="#" class="flex items-center px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-100 rounded-md">Settings</a>
            </nav>
        </div>
    </div>

    <script>
        function toggleMobileMenu() {
            document.getElementById('mobile-menu').classList.toggle('hidden');
        }
    </script>
</body>
</html>
```

---

## Form Pages

Complete form with validation.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Form Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50 py-12">
    <div class="max-w-2xl mx-auto px-4">
        <div class="bg-white rounded-lg border border-slate-200 shadow-sm">
            <div class="p-6 border-b border-slate-200">
                <h1 class="text-2xl font-bold text-slate-900">Create User</h1>
                <p class="text-sm text-slate-600 mt-1">Add a new user to your organization</p>
            </div>

            <form id="userForm" class="p-6 space-y-6">
                <!-- Name Field -->
                <div class="space-y-2">
                    <label for="name" class="block text-sm font-medium text-slate-900">
                        Name <span class="text-red-500">*</span>
                    </label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        required
                        class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-slate-900"
                        placeholder="John Doe"
                    >
                    <p id="name-error" class="hidden text-sm text-red-600"></p>
                </div>

                <!-- Email Field -->
                <div class="space-y-2">
                    <label for="email" class="block text-sm font-medium text-slate-900">
                        Email <span class="text-red-500">*</span>
                    </label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        required
                        class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-slate-900"
                        placeholder="john@example.com"
                    >
                    <p id="email-error" class="hidden text-sm text-red-600"></p>
                </div>

                <!-- Role Select -->
                <div class="space-y-2">
                    <label for="role" class="block text-sm font-medium text-slate-900">
                        Role <span class="text-red-500">*</span>
                    </label>
                    <select
                        id="role"
                        name="role"
                        required
                        class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900"
                    >
                        <option value="">Select a role</option>
                        <option value="admin">Admin</option>
                        <option value="user">User</option>
                        <option value="guest">Guest</option>
                    </select>
                    <p id="role-error" class="hidden text-sm text-red-600"></p>
                </div>

                <!-- Bio Textarea -->
                <div class="space-y-2">
                    <label for="bio" class="block text-sm font-medium text-slate-900">
                        Bio
                    </label>
                    <textarea
                        id="bio"
                        name="bio"
                        rows="4"
                        class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-slate-900"
                        placeholder="Tell us about yourself..."
                    ></textarea>
                    <p class="text-xs text-slate-500">Maximum 160 characters</p>
                </div>

                <!-- Checkbox -->
                <div class="flex items-center gap-2">
                    <input
                        type="checkbox"
                        id="notifications"
                        name="notifications"
                        class="h-4 w-4 rounded border-slate-300 text-slate-900 focus:ring-2 focus:ring-slate-900"
                    >
                    <label for="notifications" class="text-sm font-medium text-slate-900">
                        Send email notifications
                    </label>
                </div>

                <!-- Buttons -->
                <div class="flex gap-4 pt-4">
                    <button
                        type="submit"
                        class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800 focus:outline-none focus:ring-2 focus:ring-slate-900"
                    >
                        Create User
                    </button>
                    <button
                        type="button"
                        onclick="resetForm()"
                        class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100 focus:outline-none focus:ring-2 focus:ring-slate-900"
                    >
                        Cancel
                    </button>
                </div>
            </form>
        </div>

        <!-- Success Message (hidden by default) -->
        <div id="success-message" class="hidden mt-4 bg-green-50 border border-green-200 rounded-lg p-4">
            <p class="text-sm font-medium text-green-800">User created successfully!</p>
        </div>
    </div>

    <script>
        document.getElementById('userForm').addEventListener('submit', function(e) {
            e.preventDefault();

            // Clear previous errors
            document.querySelectorAll('[id$="-error"]').forEach(el => {
                el.classList.add('hidden');
                el.textContent = '';
            });

            // Get form values
            const name = document.getElementById('name').value.trim();
            const email = document.getElementById('email').value.trim();
            const role = document.getElementById('role').value;

            let hasError = false;

            // Validate name
            if (name.length < 2) {
                showError('name', 'Name must be at least 2 characters');
                hasError = true;
            }

            // Validate email
            if (!email.includes('@')) {
                showError('email', 'Please enter a valid email address');
                hasError = true;
            }

            // Validate role
            if (!role) {
                showError('role', 'Please select a role');
                hasError = true;
            }

            if (!hasError) {
                // Show success message
                document.getElementById('success-message').classList.remove('hidden');
                // Reset form
                this.reset();
                // Hide success message after 3 seconds
                setTimeout(() => {
                    document.getElementById('success-message').classList.add('hidden');
                }, 3000);
            }
        });

        function showError(fieldId, message) {
            const errorEl = document.getElementById(fieldId + '-error');
            errorEl.textContent = message;
            errorEl.classList.remove('hidden');
        }

        function resetForm() {
            document.getElementById('userForm').reset();
            document.querySelectorAll('[id$="-error"]').forEach(el => {
                el.classList.add('hidden');
            });
        }
    </script>
</body>
</html>
```

---

## Data Tables

Table with sorting and filtering.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Data Table Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50 py-12">
    <div class="max-w-6xl mx-auto px-4">
        <div class="bg-white rounded-lg border border-slate-200 shadow-sm">
            <div class="p-6 border-b border-slate-200">
                <div class="flex items-center justify-between">
                    <div>
                        <h1 class="text-2xl font-bold text-slate-900">Users</h1>
                        <p class="text-sm text-slate-600 mt-1">Manage your team members</p>
                    </div>
                    <button class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800">
                        Add User
                    </button>
                </div>

                <!-- Search -->
                <div class="mt-4">
                    <input
                        type="text"
                        id="searchInput"
                        onkeyup="filterTable()"
                        class="h-10 w-full max-w-sm rounded-md border border-slate-300 bg-white px-3 py-2 text-sm placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-slate-900"
                        placeholder="Search users..."
                    >
                </div>
            </div>

            <div class="overflow-x-auto">
                <table id="userTable" class="w-full">
                    <thead class="bg-slate-50 border-b border-slate-200">
                        <tr>
                            <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase cursor-pointer hover:bg-slate-100" onclick="sortTable(0)">
                                Name ↕
                            </th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase cursor-pointer hover:bg-slate-100" onclick="sortTable(1)">
                                Email ↕
                            </th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase">
                                Role
                            </th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase">
                                Status
                            </th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-slate-700 uppercase">
                                Actions
                            </th>
                        </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-200">
                        <tr class="hover:bg-slate-50">
                            <td class="px-6 py-4 text-sm font-medium text-slate-900">John Doe</td>
                            <td class="px-6 py-4 text-sm text-slate-600">john@example.com</td>
                            <td class="px-6 py-4 text-sm text-slate-600">Admin</td>
                            <td class="px-6 py-4">
                                <span class="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-semibold text-green-800">
                                    Active
                                </span>
                            </td>
                            <td class="px-6 py-4 text-sm">
                                <button class="text-slate-600 hover:text-slate-900 mr-3">Edit</button>
                                <button class="text-red-600 hover:text-red-900">Delete</button>
                            </td>
                        </tr>
                        <tr class="hover:bg-slate-50">
                            <td class="px-6 py-4 text-sm font-medium text-slate-900">Jane Smith</td>
                            <td class="px-6 py-4 text-sm text-slate-600">jane@example.com</td>
                            <td class="px-6 py-4 text-sm text-slate-600">User</td>
                            <td class="px-6 py-4">
                                <span class="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-semibold text-green-800">
                                    Active
                                </span>
                            </td>
                            <td class="px-6 py-4 text-sm">
                                <button class="text-slate-600 hover:text-slate-900 mr-3">Edit</button>
                                <button class="text-red-600 hover:text-red-900">Delete</button>
                            </td>
                        </tr>
                        <tr class="hover:bg-slate-50">
                            <td class="px-6 py-4 text-sm font-medium text-slate-900">Bob Johnson</td>
                            <td class="px-6 py-4 text-sm text-slate-600">bob@example.com</td>
                            <td class="px-6 py-4 text-sm text-slate-600">User</td>
                            <td class="px-6 py-4">
                                <span class="inline-flex items-center rounded-full bg-gray-100 px-2.5 py-0.5 text-xs font-semibold text-gray-800">
                                    Inactive
                                </span>
                            </td>
                            <td class="px-6 py-4 text-sm">
                                <button class="text-slate-600 hover:text-slate-900 mr-3">Edit</button>
                                <button class="text-red-600 hover:text-red-900">Delete</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <!-- Pagination -->
            <div class="p-6 border-t border-slate-200 flex items-center justify-between">
                <p class="text-sm text-slate-600">Showing 1 to 3 of 3 results</p>
                <div class="flex gap-2">
                    <button class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-3 py-1 text-sm font-medium text-slate-900 hover:bg-slate-100 disabled:opacity-50" disabled>
                        Previous
                    </button>
                    <button class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-3 py-1 text-sm font-medium text-slate-900 hover:bg-slate-100 disabled:opacity-50" disabled>
                        Next
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script>
        // Filter table based on search input
        function filterTable() {
            const input = document.getElementById('searchInput');
            const filter = input.value.toLowerCase();
            const table = document.getElementById('userTable');
            const rows = table.getElementsByTagName('tr');

            for (let i = 1; i < rows.length; i++) {
                const cells = rows[i].getElementsByTagName('td');
                let found = false;

                for (let j = 0; j < cells.length; j++) {
                    const cellText = cells[j].textContent || cells[j].innerText;
                    if (cellText.toLowerCase().indexOf(filter) > -1) {
                        found = true;
                        break;
                    }
                }

                rows[i].style.display = found ? '' : 'none';
            }
        }

        // Simple table sorting
        function sortTable(columnIndex) {
            const table = document.getElementById('userTable');
            const rows = Array.from(table.getElementsByTagName('tbody')[0].getElementsByTagName('tr'));

            rows.sort((a, b) => {
                const aText = a.getElementsByTagName('td')[columnIndex].textContent;
                const bText = b.getElementsByTagName('td')[columnIndex].textContent;
                return aText.localeCompare(bText);
            });

            const tbody = table.getElementsByTagName('tbody')[0];
            rows.forEach(row => tbody.appendChild(row));
        }
    </script>
</body>
</html>
```

---

## Modal Dialogs

Confirmation and form modals.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Modal Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50 p-12">
    <div class="max-w-4xl mx-auto">
        <h1 class="text-3xl font-bold mb-6">Modal Examples</h1>

        <div class="space-x-4">
            <button onclick="openModal('confirmModal')" class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800">
                Open Confirmation Modal
            </button>
            <button onclick="openModal('formModal')" class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100">
                Open Form Modal
            </button>
        </div>
    </div>

    <!-- Confirmation Modal -->
    <div id="confirmModal" class="hidden fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
        <div class="bg-white rounded-lg shadow-lg max-w-md w-full mx-4">
            <div class="p-6">
                <h2 class="text-lg font-semibold text-slate-900">Are you sure?</h2>
                <p class="mt-2 text-sm text-slate-600">
                    This action cannot be undone. This will permanently delete your account and remove your data from our servers.
                </p>
            </div>
            <div class="px-6 py-4 bg-slate-50 rounded-b-lg flex justify-end gap-3">
                <button onclick="closeModal('confirmModal')" class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100">
                    Cancel
                </button>
                <button onclick="handleConfirm()" class="inline-flex items-center justify-center rounded-md bg-red-600 px-4 py-2 text-sm font-medium text-white hover:bg-red-700">
                    Delete Account
                </button>
            </div>
        </div>
    </div>

    <!-- Form Modal -->
    <div id="formModal" class="hidden fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
        <div class="bg-white rounded-lg shadow-lg max-w-md w-full mx-4">
            <div class="p-6 border-b border-slate-200">
                <div class="flex items-center justify-between">
                    <h2 class="text-lg font-semibold text-slate-900">Create New Project</h2>
                    <button onclick="closeModal('formModal')" class="text-slate-400 hover:text-slate-600">
                        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                        </svg>
                    </button>
                </div>
                <p class="mt-1 text-sm text-slate-600">Add a new project to your workspace</p>
            </div>
            <form class="p-6 space-y-4">
                <div class="space-y-2">
                    <label for="projectName" class="block text-sm font-medium text-slate-900">Project Name</label>
                    <input
                        type="text"
                        id="projectName"
                        class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900"
                        placeholder="My Awesome Project"
                    >
                </div>
                <div class="space-y-2">
                    <label for="projectDesc" class="block text-sm font-medium text-slate-900">Description</label>
                    <textarea
                        id="projectDesc"
                        rows="3"
                        class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900"
                        placeholder="Brief description..."
                    ></textarea>
                </div>
            </form>
            <div class="px-6 py-4 bg-slate-50 rounded-b-lg flex justify-end gap-3">
                <button onclick="closeModal('formModal')" class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100">
                    Cancel
                </button>
                <button onclick="handleFormSubmit()" class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800">
                    Create Project
                </button>
            </div>
        </div>
    </div>

    <script>
        function openModal(modalId) {
            document.getElementById(modalId).classList.remove('hidden');
        }

        function closeModal(modalId) {
            document.getElementById(modalId).classList.add('hidden');
        }

        function handleConfirm() {
            alert('Account deleted!');
            closeModal('confirmModal');
        }

        function handleFormSubmit() {
            const projectName = document.getElementById('projectName').value;
            if (projectName) {
                alert('Project created: ' + projectName);
                closeModal('formModal');
            }
        }

        // Close modal when clicking outside
        document.querySelectorAll('[id$="Modal"]').forEach(modal => {
            modal.addEventListener('click', function(e) {
                if (e.target === this) {
                    closeModal(this.id);
                }
            });
        });

        // Close modal on ESC key
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape') {
                document.querySelectorAll('[id$="Modal"]').forEach(modal => {
                    if (!modal.classList.contains('hidden')) {
                        closeModal(modal.id);
                    }
                });
            }
        });
    </script>
</body>
</html>
```

---

## Settings Pages

Tabbed settings interface.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Settings Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50 py-12">
    <div class="max-w-4xl mx-auto px-4">
        <h1 class="text-3xl font-bold text-slate-900 mb-2">Settings</h1>
        <p class="text-slate-600 mb-6">Manage your account settings and preferences</p>

        <!-- Tabs -->
        <div class="border-b border-slate-200 mb-6">
            <nav class="flex gap-8">
                <button onclick="switchTab('general')" class="tab-button py-4 text-sm font-medium border-b-2 border-slate-900 text-slate-900">
                    General
                </button>
                <button onclick="switchTab('security')" class="tab-button py-4 text-sm font-medium text-slate-600 hover:text-slate-900">
                    Security
                </button>
                <button onclick="switchTab('notifications')" class="tab-button py-4 text-sm font-medium text-slate-600 hover:text-slate-900">
                    Notifications
                </button>
            </nav>
        </div>

        <!-- General Tab -->
        <div id="general-tab" class="tab-content space-y-6">
            <div class="bg-white rounded-lg border border-slate-200 shadow-sm">
                <div class="p-6 border-b border-slate-200">
                    <h2 class="text-lg font-semibold text-slate-900">Profile</h2>
                    <p class="text-sm text-slate-600 mt-1">Update your personal information</p>
                </div>
                <div class="p-6 space-y-4">
                    <div class="space-y-2">
                        <label class="block text-sm font-medium text-slate-900">Name</label>
                        <input type="text" value="John Doe" class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900">
                    </div>
                    <div class="space-y-2">
                        <label class="block text-sm font-medium text-slate-900">Email</label>
                        <input type="email" value="john@example.com" class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900">
                    </div>
                    <button class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800">
                        Save Changes
                    </button>
                </div>
            </div>
        </div>

        <!-- Security Tab -->
        <div id="security-tab" class="tab-content hidden space-y-6">
            <div class="bg-white rounded-lg border border-slate-200 shadow-sm">
                <div class="p-6 border-b border-slate-200">
                    <h2 class="text-lg font-semibold text-slate-900">Password</h2>
                    <p class="text-sm text-slate-600 mt-1">Change your password to keep your account secure</p>
                </div>
                <div class="p-6 space-y-4">
                    <div class="space-y-2">
                        <label class="block text-sm font-medium text-slate-900">Current Password</label>
                        <input type="password" class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900">
                    </div>
                    <div class="space-y-2">
                        <label class="block text-sm font-medium text-slate-900">New Password</label>
                        <input type="password" class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900">
                    </div>
                    <button class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800">
                        Update Password
                    </button>
                </div>
            </div>

            <div class="bg-white rounded-lg border border-red-200 shadow-sm">
                <div class="p-6 border-b border-red-200">
                    <h2 class="text-lg font-semibold text-red-600">Danger Zone</h2>
                    <p class="text-sm text-slate-600 mt-1">Irreversible and destructive actions</p>
                </div>
                <div class="p-6">
                    <button class="inline-flex items-center justify-center rounded-md bg-red-600 px-4 py-2 text-sm font-medium text-white hover:bg-red-700">
                        Delete Account
                    </button>
                </div>
            </div>
        </div>

        <!-- Notifications Tab -->
        <div id="notifications-tab" class="tab-content hidden">
            <div class="bg-white rounded-lg border border-slate-200 shadow-sm">
                <div class="p-6 border-b border-slate-200">
                    <h2 class="text-lg font-semibold text-slate-900">Email Notifications</h2>
                    <p class="text-sm text-slate-600 mt-1">Choose what emails you want to receive</p>
                </div>
                <div class="p-6 space-y-6">
                    <div class="flex items-center justify-between">
                        <div>
                            <label class="text-sm font-medium text-slate-900">Marketing emails</label>
                            <p class="text-sm text-slate-600">Receive emails about new products and features</p>
                        </div>
                        <label class="relative inline-flex items-center cursor-pointer">
                            <input type="checkbox" class="sr-only peer">
                            <div class="w-11 h-6 bg-slate-200 peer-focus:ring-2 peer-focus:ring-slate-900 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-slate-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-slate-900"></div>
                        </label>
                    </div>
                    <div class="border-t border-slate-200"></div>
                    <div class="flex items-center justify-between">
                        <div>
                            <label class="text-sm font-medium text-slate-900">Security alerts</label>
                            <p class="text-sm text-slate-600">Receive notifications about account security</p>
                        </div>
                        <label class="relative inline-flex items-center cursor-pointer">
                            <input type="checkbox" checked class="sr-only peer">
                            <div class="w-11 h-6 bg-slate-200 peer-focus:ring-2 peer-focus:ring-slate-900 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-slate-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-slate-900"></div>
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function switchTab(tabName) {
            // Hide all tabs
            document.querySelectorAll('.tab-content').forEach(el => {
                el.classList.add('hidden');
            });

            // Show selected tab
            document.getElementById(tabName + '-tab').classList.remove('hidden');

            // Update tab button styles
            document.querySelectorAll('.tab-button').forEach(el => {
                el.classList.remove('border-slate-900', 'text-slate-900');
                el.classList.add('text-slate-600');
            });
            event.target.classList.add('border-b-2', 'border-slate-900', 'text-slate-900');
        }
    </script>
</body>
</html>
```

---

## Login/Signup Pages

Authentication pages.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50 flex items-center justify-center min-h-screen">
    <div class="bg-white rounded-lg border border-slate-200 shadow-sm w-full max-w-md mx-4">
        <div class="p-6 space-y-1">
            <h1 class="text-2xl font-bold text-slate-900">Sign in</h1>
            <p class="text-sm text-slate-600">Enter your email and password to access your account</p>
        </div>

        <form class="p-6 pt-0 space-y-4">
            <div class="space-y-2">
                <label for="email" class="block text-sm font-medium text-slate-900">Email</label>
                <input
                    type="email"
                    id="email"
                    class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-slate-900"
                    placeholder="name@example.com"
                >
            </div>

            <div class="space-y-2">
                <div class="flex items-center justify-between">
                    <label for="password" class="block text-sm font-medium text-slate-900">Password</label>
                    <a href="#" class="text-sm text-slate-600 hover:text-slate-900">Forgot password?</a>
                </div>
                <input
                    type="password"
                    id="password"
                    class="h-10 w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900"
                >
            </div>

            <button type="submit" class="w-full inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800 focus:outline-none focus:ring-2 focus:ring-slate-900">
                Sign in
            </button>
        </form>

        <div class="p-6 pt-0">
            <p class="text-center text-sm text-slate-600">
                Don't have an account?
                <a href="#" class="text-slate-900 hover:underline font-medium">Sign up</a>
            </p>
        </div>
    </div>
</body>
</html>
```

---

## Landing Pages

Simple hero section with features.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Landing Page Prototype</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-white">
    <!-- Header -->
    <header class="border-b border-slate-200">
        <div class="max-w-6xl mx-auto px-4 py-4 flex items-center justify-between">
            <div class="text-xl font-bold text-slate-900">Brand</div>
            <nav class="hidden md:flex items-center gap-6">
                <a href="#" class="text-sm font-medium text-slate-600 hover:text-slate-900">Features</a>
                <a href="#" class="text-sm font-medium text-slate-600 hover:text-slate-900">Pricing</a>
                <a href="#" class="text-sm font-medium text-slate-600 hover:text-slate-900">About</a>
            </nav>
            <button class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800">
                Get Started
            </button>
        </div>
    </header>

    <!-- Hero Section -->
    <section class="py-20 px-4">
        <div class="max-w-4xl mx-auto text-center">
            <h1 class="text-5xl font-bold text-slate-900 mb-6">
                Build amazing products faster
            </h1>
            <p class="text-xl text-slate-600 mb-8">
                The best platform for creating modern web applications. Start building today with our powerful tools and features.
            </p>
            <div class="flex gap-4 justify-center">
                <button class="inline-flex items-center justify-center rounded-md bg-slate-900 px-6 py-3 text-sm font-medium text-white hover:bg-slate-800">
                    Start Free Trial
                </button>
                <button class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-6 py-3 text-sm font-medium text-slate-900 hover:bg-slate-100">
                    Learn More
                </button>
            </div>
        </div>
    </section>

    <!-- Features Section -->
    <section class="py-20 px-4 bg-gray-50">
        <div class="max-w-6xl mx-auto">
            <h2 class="text-3xl font-bold text-center text-slate-900 mb-12">Features</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
                <div class="bg-white rounded-lg border border-slate-200 p-6">
                    <h3 class="text-lg font-semibold text-slate-900 mb-2">Fast Performance</h3>
                    <p class="text-slate-600">Lightning-fast load times and optimized performance for the best user experience.</p>
                </div>
                <div class="bg-white rounded-lg border border-slate-200 p-6">
                    <h3 class="text-lg font-semibold text-slate-900 mb-2">Easy to Use</h3>
                    <p class="text-slate-600">Intuitive interface designed for developers and non-developers alike.</p>
                </div>
                <div class="bg-white rounded-lg border border-slate-200 p-6">
                    <h3 class="text-lg font-semibold text-slate-900 mb-2">Secure</h3>
                    <p class="text-slate-600">Enterprise-grade security to keep your data safe and protected.</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Footer -->
    <footer class="border-t border-slate-200 py-8 px-4">
        <div class="max-w-6xl mx-auto text-center text-sm text-slate-600">
            © 2024 Brand. All rights reserved.
        </div>
    </footer>
</body>
</html>
```

---

## Component Library

Quick reference for common components.

### Buttons
```html
<!-- Primary -->
<button class="inline-flex items-center justify-center rounded-md bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800 focus:outline-none focus:ring-2 focus:ring-slate-900">
    Button
</button>

<!-- Secondary/Outline -->
<button class="inline-flex items-center justify-center rounded-md border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100">
    Button
</button>

<!-- Ghost -->
<button class="inline-flex items-center justify-center rounded-md px-4 py-2 text-sm font-medium text-slate-900 hover:bg-slate-100">
    Button
</button>

<!-- Destructive -->
<button class="inline-flex items-center justify-center rounded-md bg-red-600 px-4 py-2 text-sm font-medium text-white hover:bg-red-700">
    Delete
</button>
```

### Badges
```html
<!-- Default -->
<span class="inline-flex items-center rounded-full bg-slate-100 px-2.5 py-0.5 text-xs font-semibold text-slate-800">Badge</span>

<!-- Success -->
<span class="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-semibold text-green-800">Active</span>

<!-- Warning -->
<span class="inline-flex items-center rounded-full bg-yellow-100 px-2.5 py-0.5 text-xs font-semibold text-yellow-800">Pending</span>

<!-- Error -->
<span class="inline-flex items-center rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-semibold text-red-800">Failed</span>
```

### Alerts
```html
<!-- Info -->
<div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
    <p class="text-sm font-medium text-blue-800">Info message</p>
</div>

<!-- Success -->
<div class="bg-green-50 border border-green-200 rounded-lg p-4">
    <p class="text-sm font-medium text-green-800">Success message</p>
</div>

<!-- Warning -->
<div class="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
    <p class="text-sm font-medium text-yellow-800">Warning message</p>
</div>

<!-- Error -->
<div class="bg-red-50 border border-red-200 rounded-lg p-4">
    <p class="text-sm font-medium text-red-800">Error message</p>
</div>
```

### Avatar
```html
<!-- With initials -->
<div class="w-10 h-10 rounded-full bg-slate-900 flex items-center justify-center text-white text-sm font-medium">
    JD
</div>

<!-- With image -->
<img src="avatar.jpg" alt="User" class="w-10 h-10 rounded-full">
```

---

**Tips:**
- All templates are standalone and ready to use
- Just copy, paste, and customize
- No build process or dependencies needed
- Responsive by default
- Accessible with semantic HTML
