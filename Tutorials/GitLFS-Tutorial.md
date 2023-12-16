# Git-LFS Tutorial

## Overview

The [git-lfs](https://git-lfs.com/) package can be used for handling large assets using Git. This is important when working with Wwise and Unity.

Git-LFS recognizes the types of files to include based on information provided via the [.gitattributes](https://www.git-scm.com/docs/gitattributes) file. In particular, it specifies what types of files should be tracked using Git-LFS. For instance, the following line will track all .wav files:
> `*.wav filter=lfs diff=lfs merge=lfs -text`

Git-LFS uploads large files to an online storage destination. Links to this storage location will be added to the remote Git repo, and will be downloaded when performing a pull request to a local repo.


## Installation and Workflow

*Note that the following workflow was written for macOS. Some of the installation steps will vary when working with a different OS. However, the **.gitignore** and **.gitattributes** files will be the same regardless of the OS.*

1. Install Git-LFS using Homebrew:
    > `brew install git-lfs`

    * If you fo not have Homebrew installed (you'll know depending on whether the previous command failed), you can do so via the CLI using the command provided under the *Install Homebrew* section of the [Homebrew site](https://brew.sh/).

2. Enable Git-LFS on your repo by navigating to the root directory of your local repo, and using the following command:
    > `git lfs install`

3. You can now use the **.gitattributes** file to specify what kinds of files to track.

    * You can copy [this .gitattributes](../.gitattributes) onto your repo. This files has been written for working with Wwise and Unity.

    * You can initialize your own **.gitattributes** and specify what kinds of files to track via the following command:
        > `git lfs track "*.wav"`

4. You can now add, commit, and push your files. If the push fails, try the following steps:
    1. Reenable git-lfs for your repo.
        > `git lfs install`

    2. Push your lfs file directly
        > `git lfs push --all origin`

    3. Try and push your most recent commit once again
        > `git push`

### Notes
* Defining the file types Git LFS should track will not, by itself, convert any pre-existing files to Git LFS, such as files on other branches or in your prior commit history. To do that, use the [git lfs migrate(1)](https://github.com/git-lfs/git-lfs/blob/main/docs/man/git-lfs-migrate.adoc?utm_source=gitlfs_site&utm_medium=doc_man_migrate_link&utm_campaign=gitlfs) command, which has a range of options designed to suit various potential use cases.

* GitHub grants 1GiB a month of free storage and bandwidth per repo. You can always [purchase and manage](https://docs.github.com/en/billing/managing-billing-for-git-large-file-storage) the Git LFS storage capacity.

## Resources
[Git LFS - Homepage](https://git-lfs.com/)

[GitHub - Configuring Git Large File Storage](https://docs.github.com/en/repositories/working-with-files/managing-large-files/configuring-git-large-file-storage)

[GitHub - About storage and bandwidth usage](https://docs.github.com/en/repositories/working-with-files/managing-large-files/about-storage-and-bandwidth-usage)

[Atlassian - Git LFS Tutorial](https://www.atlassian.com/git/tutorials/git-lfs)