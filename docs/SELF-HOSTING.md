# Lex Self-Hosting Administration Guide

**For:** Non-technical administrators and IT staff
**Level:** Beginner-friendly with advanced sections
**Last Updated:** April 2026

---

## Table of Contents

1. [System Requirements](#system-requirements)
2. [Installation](#installation)
3. [First-Time Setup](#first-time-setup)
4. [Daily Operations](#daily-operations)
5. [User Management](#user-management)
6. [Backup & Recovery](#backup--recovery)
7. [Troubleshooting](#troubleshooting)
8. [Security Hardening](#security-hardening)
9. [Performance Optimization](#performance-optimization)
10. [Getting Help](#getting-help)

---

## System Requirements

### Minimum Specifications

| Component | Requirement | Notes |
|-----------|-------------|-------|
| **CPU** | 4 cores | More cores = more concurrent users |
| **RAM** | 8 GB | 16 GB recommended for 500+ users |
| **Storage** | 100 GB | 200+ GB if storing many files |
| **OS** | Linux (Ubuntu 22.04+) or Windows Server 2019+ | |
| **Internet** | Broadband (10+ Mbps) | For Google integration |
| **Browser** | Chrome 90+, Firefox 88+, Safari 14+, Edge 90+ | Any modern browser works |

### Network Requirements

- **Open Ports:**
  - 443 (HTTPS) — Web interface (required)
  - 80 (HTTP) — Redirect to HTTPS (recommended)
  - 5432 — PostgreSQL (internal only)
  - 9000 — MinIO console (admin only)

- **SSL Certificate:**
  - Self-signed: Auto-generated (insecure, dev only)
  - Let's Encrypt: Free, auto-renewing (recommended)
  - Commercial CA: Purchase from provider

### Storage Planning

**Calculate storage needs:**

```
Base installation:        ~5 GB
Per 1000 users:          ~2 GB (profiles, settings)
Per 1000 assessments:    ~1 GB (questions, snapshots)
Per 1 GB of files:       ~1 GB (attachments, resources)
```

**Example:** 500 users + 100 assessments + 50 GB files = ~60 GB total

---

## Installation

### Prerequisites Checklist

- ✅ Server with internet access
- ✅ Root or sudo access
- ✅ 8+ GB RAM free
- ✅ 100+ GB disk space free
- ✅ Port 443 available (or reverse proxy configured)
- ✅ DNS record pointing to server IP (e.g., lex.school.edu)

### Quick Installation (5-10 minutes)

**Step 1: Download & Execute Bootstrap Script**

```bash
# SSH into your server
ssh admin@your-server-ip

# Download installer (runs as interactive script)
curl -fsSL https://releases.lex.edu/install-latest.sh > install.sh
chmod +x install.sh

# Run installer (will prompt for configuration)
sudo ./install.sh
```

**Step 2: Follow Interactive Prompts**

The script will ask:

```
1. Institution Name: [Your School]
2. Admin Email: admin@school.edu
3. Admin Password: [hidden input]
4. Domain Name: lex.school.edu
5. SSL Certificate:
   a) Auto-generate self-signed (dev only)
   b) Use Let's Encrypt (recommended)
   c) Import existing certificate
6. File Storage Size: [100 GB]
7. Backup Location: [/mnt/backup]
8. Timezone: [UTC]
9. Google Drive Integration: Yes/No
```

**Step 3: Wait for Deployment**

The script will:
- Install Docker and Docker Compose
- Download Lex container images
- Initialize PostgreSQL database
- Generate SSL certificates
- Start all services
- Run health checks

**Expected output:**
```
✓ System requirements verified
✓ Docker installed successfully
✓ PostgreSQL initialized
✓ Keycloak configured
✓ Lex services started
✓ Health checks passed

🎉 Installation complete!
   Access Lex at: https://lex.school.edu
   Admin email: admin@school.edu
   Initial password: [unique-password-sent-to-email]
```

### Manual Installation (Advanced)

If you prefer more control or the quick script fails:

**Step 1: Install Dependencies**
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install -y docker.io docker-compose

# Start Docker
sudo systemctl start docker
sudo systemctl enable docker
```

**Step 2: Download Lex Files**
```bash
cd /opt
sudo git clone https://github.com/your-org/lex.git
cd lex
```

**Step 3: Configure Environment**
```bash
# Copy example config
sudo cp .env.example .env

# Edit configuration
sudo nano .env

# Set these values:
# INSTITUTION_NAME=Your School
# ADMIN_EMAIL=admin@school.edu
# DOMAIN=lex.school.edu
# DATABASE_PASSWORD=[strong-password]
```

**Step 4: Start Services**
```bash
# Start in background
sudo docker-compose up -d

# Watch startup logs
sudo docker-compose logs -f

# Should see: "Lex is ready" after 2-3 minutes
```

**Step 5: Verify Installation**
```bash
# Check if accessible
curl https://lex.school.edu

# Check service status
sudo docker-compose ps

# All services should show "Up"
```

---

## First-Time Setup

### Access the Admin Panel

1. Open browser: `https://lex.school.edu`
2. Click "Login" (or auto-redirects)
3. Enter admin credentials:
   - Email: `admin@school.edu`
   - Password: Check email or terminal output
4. Change password immediately:
   - Profile → Change Password
   - Use strong password (12+ characters, mix of upper/lower/numbers/symbols)

### Initial Configuration

#### 1. Institutional Settings

**Menu:** Admin → Institution Settings

- **Institution Name:** Your school/university name
- **Logo:** Upload institution logo (512×512 PNG)
- **Primary Color:** Choose brand color
- **Support Email:** admin@school.edu
- **Support Phone:** Contact number

#### 2. Email Configuration (SMTP)

**Menu:** Admin → Settings → Email

Configure email to send notifications:

```
SMTP Server: smtp.gmail.com
SMTP Port: 587
Use TLS: Yes
Username: noreply@school.edu
Password: [app-specific password]
From Address: noreply@school.edu
From Name: Lex Learning Platform
```

**Test email:**
- Click "Send Test Email"
- Check inbox for test message

#### 3. Create User Accounts

**Menu:** Admin → Users → Create User

Create accounts for:
- ✅ Institution admin(s)
- ✅ Department heads/coordinators
- ✅ Teachers (can do this or bulk import)
- ✅ Students (can do this or bulk import)

**For each user:**
- Email: email@school.edu
- Full Name: First Last
- Role: Student/Teacher/Admin
- Department: Math/Science/etc. (optional)
- Initial Password: Auto-generated (sent via email)

#### 4. Bulk Import Users (Recommended)

**Menu:** Admin → Users → Import

**Option A: CSV File**

Create CSV (file.csv):
```csv
email,firstName,lastName,role,department
john.doe@school.edu,John,Doe,Teacher,Mathematics
jane.smith@school.edu,Jane,Smith,Student,
```

Upload file → Review mappings → Click "Import"

**Option B: Google Sheets**

Connect Google account → Select sheet → Auto-import

#### 5. Create Academic Structure

**Menu:** Admin → Schedule → Academic Years

1. **Create Academic Year**
   - Year: 2024
   - Start Date: 2024-04-01
   - End Date: 2025-03-31

2. **Create Terms**
   - Term 1: April-July
   - Term 2: August-November
   - Term 3: December-March

3. **Create Periods/Slots**
   - For each day/time combination
   - Assign teachers
   - Assign classrooms

4. **Create Classes**
   - Grade/Class: 10-A
   - Section Head: Teacher Name
   - Room: 101
   - Add students

---

## Daily Operations

### Dashboard Overview

**Login each morning → Review Dashboard**

The dashboard shows:
- 🔴 Alerts (system health, pending actions)
- 📊 Activity (logins, submissions, completions)
- ⏰ Upcoming events
- 📋 Pending approvals (diary entries, etc.)

### Common Daily Tasks

#### Managing Pending Approvals

**Menu:** Dashboard → Pending Approvals
(Or specific module: Diary → Approvals)

- Review teacher diary entries
- Approve (publish) or reject
- Leave comments if needed
- Bulk approve similar items

#### Monitoring System Health

**Menu:** Admin → System Health

Check:
- ✅ Database: Should show "Connected"
- ✅ File Storage: Should show "Available" with space used
- ✅ Email Service: Should show "Connected"
- ✅ Backup Status: Last backup time and size
- ⚠️ Any errors listed: Click for details

**If issues appear:**
1. Click the issue
2. Click "Troubleshoot"
3. Follow suggested steps
4. Or contact support (see [Getting Help](#getting-help))

#### Viewing Activity Logs

**Menu:** Admin → Audit Logs

Filter by:
- User: See what specific person did
- Action: See all logins, deletions, etc.
- Date Range: Specific time period
- Result: Success/failures only

**Example uses:**
- "Who created this assessment?" → Search assessment name
- "What happened on April 1?" → Filter by date
- "Show me all failed logins" → Filter by action = "Login" + result = "Failed"

### Periodic Tasks (Weekly)

- [ ] Check audit logs for unusual activity
- [ ] Review system health dashboard
- [ ] Verify recent backups completed
- [ ] Check disk space (should have 20%+ free)

### Periodic Tasks (Monthly)

- [ ] Review user activity (active vs. inactive accounts)
- [ ] Check storage growth (trending analysis)
- [ ] Review and update password policies
- [ ] Test backup restoration process
- [ ] Review security logs for threats

---

## User Management

### Adding Users

**Menu:** Admin → Users → Create User

Fill in:
- **Email** (username, must be unique)
- **Full Name**
- **Role:** Student, Teacher, or Admin
- **Department/Subject** (optional)
- **Custom Fields** (if configured)

Click "Create" → Initial password sent to email

### Editing Users

**Menu:** Admin → Users → [Select User]

Can edit:
- ✅ Full name
- ✅ Email (if no conflicts)
- ✅ Department/subject
- ✅ Custom fields
- ✅ Status (active/disabled)

Cannot edit:
- ❌ Role (delete and recreate if needed)
- ❌ User ID
- ❌ Creation date

### Resetting Passwords

**Menu:** Admin → Users → [Select User] → Reset Password

1. Click "Reset Password"
2. User receives email with temporary link
3. User clicks link and creates new password
4. Or set temporary password manually (not recommended)

### Disabling/Enabling Accounts

**Menu:** Admin → Users → [Select User] → Status

- **Disable:** User cannot login but data preserved
- **Enable:** User can login again
- **Delete:** Permanently removes user (after 30-day archive period)

### Bulk Operations

**Menu:** Admin → Users → Bulk Actions

**Import Users:**
- Upload CSV with user data
- System validates and creates accounts

**Export Users:**
- Download list of all users
- Includes email, role, last login, etc.

**Disable Inactive Users:**
- Select minimum days inactive (e.g., 90 days)
- System disables users not logged in
- Confirmation email sent

---

## Backup & Recovery

### Automatic Daily Backups

Lex automatically backs up:
- ✅ Database (daily at 2 AM)
- ✅ User files (daily at 2 AM)
- ✅ Configuration (daily at 2 AM)
- ✅ Previous 30 backups retained

**Storage:** `/mnt/backup/` (or configured location)

### Verifying Backups

**Menu:** Admin → Backups

Shows:
- Last 10 backups with timestamps
- Size of each backup
- Completion status (✅ Success or ❌ Failed)

**If backup failed:**
1. Check disk space available
2. Check network connectivity
3. Contact support if persists

### Manual Backup

**Menu:** Admin → Backups → Create Backup

1. Click "Create Backup Now"
2. System creates backup (2-5 minutes)
3. Download if needed (large file)

**For system administrators (command line):**
```bash
sudo docker-compose exec postgres pg_dump -U lex > backup.sql
```

### Restoring from Backup

⚠️ **Warning:** Restoration will overwrite current data!

**Menu:** Admin → Backups → [Select Backup] → Restore

1. Click "Restore"
2. System prompts for confirmation
3. Choose restore options:
   - [ ] Database only
   - [ ] Files only
   - [ ] Everything (recommended)
4. Confirm
5. System restores (5-15 minutes)
6. All users logged out (must login again)

**Restore from command line (advanced):**
```bash
sudo docker-compose exec postgres psql -U lex < backup.sql
```

### Offsite Backup

To protect against server failure:

**Option 1: Download Regular Backups**
- Menu → Admin → Backups → [Select] → Download
- Store on external drive or cloud storage
- Monthly is sufficient for most schools

**Option 2: Cloud Backup (Premium)**
- Connects to AWS S3, Google Drive, or Dropbox
- Automatic daily uploads
- Available for $5-20/month depending on size

**Option 3: NFS/SMB Network Backup**
- Mount network drive
- Configure backup destination in settings
- Backups automatically saved to network location

---

## Troubleshooting

### Problem: Cannot Access Lex (Page won't load)

**Step 1: Check URL**
- Verify correct domain: `https://lex.school.edu`
- Check for typos

**Step 2: Check Server Status**
```bash
sudo docker-compose ps
```
- Should show all services as "Up"
- If any show "Exited", proceed to Step 3

**Step 3: Restart Services**
```bash
sudo docker-compose restart
```
- Wait 2 minutes
- Try accessing again

**Step 4: Check Logs**
```bash
sudo docker-compose logs --tail=50
```
- Look for error messages
- Screenshot error and contact support

### Problem: Users Cannot Login

**Step 1: Verify User Exists**
- Admin → Users → Search for user email

**Step 2: Check User Status**
- User should show "Active" (not "Disabled")
- Click "Reset Password" and send link

**Step 3: Check Email**
- Ask user if they received email
- Check spam folder
- If no email sent, check SMTP settings:
  - Admin → Settings → Email
  - Click "Test Email"

**Step 4: Check Browser**
- Clear browser cache (Ctrl+Shift+Del)
- Try different browser
- Try incognito/private mode

### Problem: Forgot Admin Password

**Step 1: SSH to Server**
```bash
ssh admin@server-ip
```

**Step 2: Reset Admin via CLI**
```bash
# Option A: Send reset link to email
sudo docker-compose exec keycloak /opt/keycloak/bin/kcadm.sh \
  update users admin -s 'emailVerified=false' \
  -r lex

# Option B: Set temporary password
sudo docker-compose exec keycloak /opt/keycloak/bin/kcadm.sh \
  set-password --username admin \
  --new-password TempPassword123! \
  -r lex
```

### Problem: Disk Space Running Out

**Step 1: Check Disk Usage**
```bash
df -h
```

**Step 2: Identify Large Directories**
```bash
du -sh /opt/lex/*
du -sh /mnt/backup/*
```

**Step 3: Options to Free Space**

**A. Delete Old Backups**
```bash
# Keep only last 10 backups
sudo ls -t /mnt/backup/ | tail -n +11 | xargs -I {} rm -rf /mnt/backup/{}
```

**B. Archive Old Files**
```bash
# Move 1-year-old files to archive
find /opt/lex/files -mtime +365 -exec mv {} /archive/ \;
```

**C. Add Storage**
- Add external drive
- Mount new location
- Configure as backup destination

**D. Purchase Cloud Storage**
- Move old assessments to cloud
- Keep active data on local storage

### Problem: Email Not Sending

**Step 1: Verify SMTP Configuration**
- Admin → Settings → Email
- Click "Test Email"
- Confirms SMTP settings work

**If test email fails:**

**Step 2: Check SMTP Settings**
```bash
# Gmail example
SMTP Server: smtp.gmail.com
SMTP Port: 587
Username: your-gmail@gmail.com
Password: [app password, NOT regular password]
TLS: Enabled
```

**Step 3: Generate Gmail App Password**
- Go to https://myaccount.google.com/app-passwords
- Create app password for "Mail" and "Linux"
- Copy password into SMTP password field

**Step 4: Check Logs**
```bash
sudo docker-compose logs email
```

### Problem: System Running Slow

**Step 1: Check Resource Usage**
```bash
# CPU and memory usage
sudo docker stats

# Disk I/O
sudo iostat -x 1 5
```

**Step 2: Identify Issues**

| Issue | Solution |
|-------|----------|
| CPU high (>80%) | Reduce concurrent users or add more cores |
| RAM high (>90%) | Increase RAM or restart services: `docker-compose restart` |
| Disk I/O high | Add faster disk (SSD) or reduce file operations |
| Database slow | Run optimization: `Admin → Settings → Optimize Database` |

**Step 3: Optimize Database**
```bash
# Manual optimization
sudo docker-compose exec postgres vacuumdb -U lex -d lex
```

### Problem: Certificate Expiring Soon

**Step 1: Check Certificate Date**
- Admin → Settings → Security → SSL Certificate
- Shows expiration date

**Step 2: Renew Certificate**

**If using Let's Encrypt (auto-renewal):**
```bash
sudo docker-compose exec nginx certbot renew --dry-run
sudo docker-compose exec nginx certbot renew
```

**If using manual certificate:**
- Purchase new certificate from CA
- Upload in Admin → Settings → Security
- Specify both certificate and private key

---

## Security Hardening

### Essential Security Settings

**Admin → Settings → Security**

1. **Password Policy**
   - Minimum length: 12 characters
   - Require uppercase: Yes
   - Require numbers: Yes
   - Require symbols: Yes
   - Expiration: 90 days
   - History: Prevent reuse of last 5 passwords

2. **Session Settings**
   - Session timeout: 30 minutes
   - Timeout warning: 5 minutes
   - Max sessions per user: 2

3. **Two-Factor Authentication (2FA)**
   - Require for admins: Yes
   - Optional for teachers: Yes
   - Optional for students: No

4. **IP Whitelisting** (Advanced)
   - Allow only specific IPs to access admin panel
   - Example: `192.168.1.0/24, 203.0.113.0/24`

### Regular Security Tasks

**Monthly:**
- [ ] Review audit logs for unusual activity
- [ ] Check user accounts that have never logged in (delete if inactive 3+ months)
- [ ] Verify SSL certificate not expiring soon

**Quarterly:**
- [ ] Force password reset for inactive accounts
- [ ] Review 2FA settings
- [ ] Test backup restoration

**Annually:**
- [ ] Security audit (external)
- [ ] Review system permissions
- [ ] Update security policies

### Data Privacy

**GDPR Compliance:**

**Menu:** Admin → Privacy → GDPR

1. **Data Request**
   - Users can request download of personal data
   - System automatically generates ZIP file
   - Includes all data in user's account

2. **Data Deletion**
   - Users can request account deletion
   - 30-day waiting period for reversal
   - After 30 days: Permanent deletion

3. **Consent Management**
   - Track user consent for data processing
   - Update privacy policy
   - Users must re-consent if policy changes

---

## Performance Optimization

### Database Optimization

**Menu:** Admin → Settings → Maintenance

Click "Optimize Database" monthly:
- Rebuilds indexes
- Recovers wasted space
- Improves query speed

**Manual (command line):**
```bash
sudo docker-compose exec postgres vacuumdb -U lex -d lex -z
```

### Cache Configuration

**Menu:** Admin → Settings → Performance

- **Query Cache:** Enable (improves repeated queries)
- **Object Cache:** 512 MB (default)
- **Session Cache:** 128 MB (default)

Increase cache if you have:
- High number of users (1000+)
- Complex reports
- Large question banks

### CDN Configuration (Advanced)

If serving many files to geographically dispersed users:

1. Setup CDN (Cloudflare, CloudFront, etc.)
2. Configure in Admin → Settings → CDN
3. CDN caches files globally
4. Improves load times for distant users

---

## Getting Help

### Support Resources

**Emergency Issues (Down/Data Loss)**
- **Phone:** +1-555-LEX-HELP
- **Email:** emergency@lex.edu
- **Response Time:** 2 hours (24/7)

**Non-Emergency Issues**
- **Email:** support@lex.edu
- **Response Time:** 24 hours (business days)
- **Ticket System:** https://support.lex.edu

**Community Support**
- **Forum:** https://community.lex.edu
- **Response Time:** 4-24 hours (volunteer-based)

### Information to Provide When Reporting Issues

1. **Problem Description:** What exactly isn't working?
2. **Steps to Reproduce:** How to make the problem happen?
3. **Screenshot/Video:** Visual proof of issue
4. **Error Message:** Exact text if available
5. **System Info:**
   ```bash
   # Run this and copy output
   sudo docker-compose ps
   sudo docker-compose logs --tail=100
   ```
6. **Timeline:** When did it start? Is it ongoing?

### Self-Help Resources

- **Lex Documentation:** https://docs.lex.edu/admin
- **Video Tutorials:** https://youtube.com/lex-education
- **FAQ:** https://faq.lex.edu
- **Changelog:** https://releases.lex.edu/changelog

### Reporting Security Issues

**Do NOT:**
- Post security issues on public forums
- Email security issues to general support

**Instead:**
- Email: security@lex.edu
- Include "SECURITY" in subject line
- Provide full details of issue
- Give us time to respond (48 hours) before public disclosure

---

## Quick Reference

### Common Commands (SSH)

```bash
# View status
sudo docker-compose ps

# View logs
sudo docker-compose logs -f

# Restart all services
sudo docker-compose restart

# Restart specific service
sudo docker-compose restart lex-api

# Stop all services
sudo docker-compose stop

# Start all services
sudo docker-compose start

# View disk usage
df -h

# View memory usage
free -h

# Check if port is listening
sudo ss -tlnp | grep 443
```

### Menu Navigation

```
📊 Dashboard
│ ├── Pending Approvals
│ ├── System Health
│ └── Activity Overview

👥 Admin
│ ├── Users
│ │  ├── Create User
│ │  ├── Import Users
│ │  ├── Export Users
│ │  └── Bulk Actions
│ ├── Classes
│ ├── Settings
│ │  ├── Email/SMTP
│ │  ├── Security
│ │  ├── Performance
│ │  ├── Branding
│ │  └── Integrations
│ ├── Backups
│ ├── Audit Logs
│ └── System Health

📅 Schedule
│ ├── Academic Years
│ ├── Terms
│ ├── Periods
│ └── Classes

[Other modules...]
```

---

**Last Updated:** April 2026
**Version:** 1.0
**Next Review:** Q3 2026

For updates, visit: https://docs.lex.edu/admin
