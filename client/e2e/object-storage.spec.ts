import { expect, test } from "@playwright/test";

test.describe("Object Storage", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("/files");
  });

  test("should display file list", async ({ page }) => {
    await expect(page.locator("text=No files found")).toBeVisible();
  });

  test("should upload a file", async ({ page }) => {
    const fileChooserPromise = page.waitForEvent("filechooser");
    await page.click("text=Upload File");
    const fileChooser = await fileChooserPromise;

    await fileChooser.setFiles({
      name: "test-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("this is a test file content"),
    });

    await expect(page.locator("text=test-file.txt")).toBeVisible({ timeout: 10000 });
  });

  test("should delete a file", async ({ page }) => {
    await page.locator("button[title='Delete']").first().click();
    await expect(page.locator("text=test-file.txt")).not.toBeVisible();
  });
});
