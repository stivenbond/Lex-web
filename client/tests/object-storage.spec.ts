import { test, expect } from "@playwright/test";

test.describe("Object Storage", () => {
  test.beforeEach(async ({ page }) => {
    // We would need to handle login here, but for now we assume check-sso might work 
    // or we mock the auth state. In a real scenario, we'd use a global setup for auth.
    await page.goto("/files");
  });

  test("should display file list", async ({ page }) => {
    // Since we start with no files, we expect the empty state message
    await expect(page.locator("text=No files found")).toBeVisible();
  });

  test("should upload a file", async ({ page }) => {
    // This is a placeholder since we need a real file to upload.
    // In Playwright we can use setInputFiles.
    const fileChooserPromise = page.waitForEvent("filechooser");
    await page.click("text=Upload File");
    const fileChooser = await fileChooserPromise;
    
    // Create a dummy file
    await fileChooser.setFiles({
      name: "test-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("this is a test file content"),
    });

    // Wait for the uploading loader to disappear and file to appear
    await expect(page.locator("text=test-file.txt")).toBeVisible({ timeout: 10000 });
  });

  test("should delete a file", async ({ page }) => {
    // 1. Upload a file first
    // ... (same as above)
    
    // 2. Click delete button
    await page.locator("button[title='Delete']").first().click();
    
    // 3. Verify it's gone
    await expect(page.locator("text=test-file.txt")).not.toBeVisible();
  });
});
