using System;
using System.Collections;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TestExample.Model;
using TestExample.Utilities;

namespace TestExample.UserControls
{
    public class TreeListExpandHeaderControlViewModel:ViewModelBase
    {
        private TreeListLevelDto _levelDto= new TreeListLevelDto();
        /// <summary>
        /// 数据源总层级
        /// </summary>
        private int _totalLevel;

        public Func<IList> GetItemSourcesFunc;

        public void Init()
        {
            ResetExpandState(true);
        }

        public ICommand ExpandChangeCommand => new RelayCommand<int>(level =>
        {
            if (!bool.TryParse(ReflectHelper.GetPropertyValue(LevelDto, $"IsLevel{level}Expanded").ToString(), out bool isExpaned)) return;
            var itemSources = GetItemSourcesFunc();
            ResetLevelExpandState(itemSources, 0, level, !isExpaned);
            ResetExpandState();
        });


        public TreeListLevelDto LevelDto
        {
            get => _levelDto;
            set
            {
                if (_levelDto == value) return;
                _levelDto = value;
                RaisePropertyChanged(nameof(LevelDto));
            }
        }

        /// <summary>
        /// 重置层级展开状态
        /// </summary>
        /// <param name="itemSources"></param>
        /// <param name="currentLevel"></param>
        /// <param name="targetLevel"></param>
        /// <param name="isExpanded"></param>
        private void ResetLevelExpandState(IList itemSources, int currentLevel, int targetLevel, bool isExpanded)
        {
            var level = currentLevel + 1;
            if (level >= _totalLevel) return;
            foreach (var item in itemSources)
            {
                var childrens = ReflectHelper.GetPropertyValue(item, "Childrens") as IList;
                if (childrens == null || childrens.Count == 0) continue;

                if (level == targetLevel)
                {
                    //层级为目标层级，设置IsExpanded为待设置的IsExpanded
                    ReflectHelper.SetPropertyValue(item, "IsExpanded",isExpanded);

                    //前面的级别收起时后面的级别必须收起
                    if (isExpanded) continue;
                    if (targetLevel < _totalLevel)
                    {
                        ResetLevelExpandState(childrens, level, targetLevel + 1, false);
                    }
                }
                else if (level < targetLevel)
                {
                    if (isExpanded)
                    {
                        //后面的级别展开时前面的级别必须展开
                        ReflectHelper.SetPropertyValue(item, "IsExpanded", true);
                    }
                    ResetLevelExpandState(childrens, level, targetLevel, isExpanded);
                }
            }
        }

        /// <summary>
        /// 重置展开状态
        /// </summary>
        /// <param name="needReCalcLevel">是否需要重新计算总层级</param>
        public void ResetExpandState(bool needReCalcLevel=false)
        {
            var itemSources = GetItemSourcesFunc();
            if (needReCalcLevel)
            {
                _totalLevel = GetTreeTotalLevel(itemSources);
            }
            //设置每一层级是否显示以及展开或收拢状态
            LevelDto.IsLevel1Visible = _totalLevel > 1;
            LevelDto.IsLevel1Expanded = _totalLevel > 1 && GetSpecifiedLevelExpandState(itemSources, 0, 1);
            LevelDto.IsLevel2Visible = _totalLevel > 2;
            LevelDto.IsLevel2Expanded = _totalLevel > 2 && GetSpecifiedLevelExpandState(itemSources, 0, 2);
            LevelDto.IsLevel3Visible = _totalLevel > 3;
            LevelDto.IsLevel3Expanded = _totalLevel > 3 && GetSpecifiedLevelExpandState(itemSources, 0, 3);
            LevelDto.IsLevel4Visible = _totalLevel > 4;
            LevelDto.IsLevel4Expanded = _totalLevel > 4 && GetSpecifiedLevelExpandState(itemSources, 0, 4);
            LevelDto.IsLevel5Visible = _totalLevel > 5;
            LevelDto.IsLevel5Expanded = _totalLevel > 5 && GetSpecifiedLevelExpandState(itemSources, 0, 5);
            LevelDto.IsLevel6Visible = _totalLevel > 6;
            LevelDto.IsLevel6Expanded = _totalLevel > 6 && GetSpecifiedLevelExpandState(itemSources, 0, 6);
            LevelDto.IsLevel7Visible = _totalLevel > 7;
            LevelDto.IsLevel7Expanded = _totalLevel > 7 && GetSpecifiedLevelExpandState(itemSources, 0, 7);
            LevelDto.IsLevel8Visible = _totalLevel > 8;
            LevelDto.IsLevel8Expanded = _totalLevel > 8 && GetSpecifiedLevelExpandState(itemSources, 0, 8);
            LevelDto.IsLevel9Visible = _totalLevel > 9;
            LevelDto.IsLevel9Expanded = _totalLevel > 9 && GetSpecifiedLevelExpandState(itemSources, 0, 9);
        }

        /// <summary>
        /// 获取树总级别
        /// </summary>
        /// <param name="itemSources"></param>
        /// <returns></returns>
        private int GetTreeTotalLevel(IList itemSources)
        {
            var maxLevel = 0;
            foreach (var item in itemSources)
            {
                var level = 0;

                var childrens = ReflectHelper.GetPropertyValue(item, "Childrens") as IList;
                if (childrens != null && childrens.Count > 0)
                {
                    level += 1 + GetTreeTotalLevel(childrens);
                }
                else
                {
                    level += 1;
                }

                if (level > maxLevel)
                {
                    maxLevel = level;
                }
            }
            return maxLevel;
        }

        /// <summary>
        /// 获取指定级别展开状态
        /// </summary>
        /// <param name="itemSources"></param>
        /// <param name="level"></param>
        /// <param name="targetLevel"></param>
        /// <returns></returns>
        private bool GetSpecifiedLevelExpandState(IList itemSources, int level, int targetLevel)
        {
            var isAllExpaned = true;
            var currentLevel = level + 1;
            foreach (var item in itemSources)
            {
                if (isAllExpaned)
                {
                    var childrens = ReflectHelper.GetPropertyValue(item, "Childrens") as IList;
                    if (currentLevel == targetLevel)
                    {
                        if (childrens == null || childrens.Count <= 0) continue;

                        var objIsExpanded = ReflectHelper.GetPropertyValue(item, "IsExpanded");
                        if (!bool.TryParse(objIsExpanded?.ToString(), out bool isExpanded)) continue;
                        if (!isExpanded)
                        {
                            isAllExpaned = false;
                        }
                    }
                    else if (currentLevel < targetLevel)
                    {
                        if (childrens != null && childrens.Count > 0)
                        {
                            isAllExpaned = GetSpecifiedLevelExpandState(childrens, currentLevel, targetLevel);
                        }
                    }
                }


            }
            return isAllExpaned;
        }
    }

    /// <summary>
    /// 树形结构层级Dto
    /// </summary>
    public class TreeListLevelDto:ModelBase
    {
        private bool _isLevel1Expanded;
        private bool _isLevel2Expanded;
        private bool _isLevel3Expanded;
        private bool _isLevel4Expanded;
        private bool _isLevel5Expanded;
        private bool _isLevel6Expanded;
        private bool _isLevel7Expanded;
        private bool _isLevel8Expanded;
        private bool _isLevel9Expanded;

        private bool _isLevel1Visible;
        private bool _isLevel2Visible;
        private bool _isLevel3Visible;
        private bool _isLevel4Visible;
        private bool _isLevel5Visible;
        private bool _isLevel6Visible;
        private bool _isLevel7Visible;
        private bool _isLevel8Visible;
        private bool _isLevel9Visible;

        public bool IsLevel1Expanded
        {
            get => _isLevel1Expanded;
            set
            {
                if (_isLevel1Expanded == value)
                    return;
                _isLevel1Expanded = value;
                RaisePropertyChanged(nameof(IsLevel1Expanded));
            }
        }

        public bool IsLevel2Expanded
        {
            get => _isLevel2Expanded;
            set
            {
                if (_isLevel2Expanded == value)
                    return;
                _isLevel2Expanded = value;
                RaisePropertyChanged(nameof(IsLevel2Expanded));
            }
        }

        public bool IsLevel3Expanded
        {
            get => _isLevel3Expanded;
            set
            {
                if (_isLevel3Expanded == value)
                    return;
                _isLevel3Expanded = value;
                RaisePropertyChanged(nameof(IsLevel3Expanded));
            }
        }

        public bool IsLevel4Expanded
        {
            get => _isLevel4Expanded;
            set
            {
                if (_isLevel4Expanded == value)
                    return;
                _isLevel4Expanded = value;
                RaisePropertyChanged(nameof(IsLevel4Expanded));
            }
        }

        public bool IsLevel5Expanded
        {
            get => _isLevel5Expanded;
            set
            {
                if (_isLevel5Expanded == value)
                    return;
                _isLevel5Expanded = value;
                RaisePropertyChanged(nameof(IsLevel5Expanded));
            }
        }

        public bool IsLevel6Expanded
        {
            get => _isLevel6Expanded;
            set
            {
                if (_isLevel6Expanded == value)
                    return;
                _isLevel6Expanded = value;
                RaisePropertyChanged(nameof(IsLevel6Expanded));
            }
        }

        public bool IsLevel7Expanded
        {
            get => _isLevel7Expanded;
            set
            {
                if (_isLevel7Expanded == value)
                    return;
                _isLevel7Expanded = value;
                RaisePropertyChanged(nameof(IsLevel7Expanded));
            }
        }

        public bool IsLevel8Expanded
        {
            get => _isLevel8Expanded;
            set
            {
                if (_isLevel8Expanded == value)
                    return;
                _isLevel8Expanded = value;
                RaisePropertyChanged(nameof(IsLevel8Expanded));
            }
        }

        public bool IsLevel9Expanded
        {
            get => _isLevel9Expanded;
            set
            {
                if (_isLevel9Expanded == value)
                    return;
                _isLevel9Expanded = value;
                RaisePropertyChanged(nameof(IsLevel9Expanded));
            }
        }

        public bool IsLevel1Visible
        {
            get => _isLevel1Visible;
            set
            {
                if (_isLevel1Visible == value)
                    return;
                _isLevel1Visible = value;
                RaisePropertyChanged(nameof(IsLevel1Visible));
            }
        }

        public bool IsLevel2Visible
        {
            get => _isLevel2Visible;
            set
            {
                if (_isLevel2Visible == value)
                    return;
                _isLevel2Visible = value;
                RaisePropertyChanged(nameof(IsLevel2Visible));
            }
        }

        public bool IsLevel3Visible
        {
            get => _isLevel3Visible;
            set
            {
                if (_isLevel3Visible == value)
                    return;
                _isLevel3Visible = value;
                RaisePropertyChanged(nameof(IsLevel3Visible));
            }
        }

        public bool IsLevel4Visible
        {
            get => _isLevel4Visible;
            set
            {
                if (_isLevel4Visible == value)
                    return;
                _isLevel4Visible = value;
                RaisePropertyChanged(nameof(IsLevel4Visible));
            }
        }

        public bool IsLevel5Visible
        {
            get => _isLevel5Visible;
            set
            {
                if (_isLevel5Visible == value)
                    return;
                _isLevel5Visible = value;
                RaisePropertyChanged(nameof(IsLevel5Visible));
            }
        }

        public bool IsLevel6Visible
        {
            get => _isLevel6Visible;
            set
            {
                if (_isLevel6Visible == value)
                    return;
                _isLevel6Visible = value;
                RaisePropertyChanged(nameof(IsLevel6Visible));
            }
        }

        public bool IsLevel7Visible
        {
            get => _isLevel7Visible;
            set
            {
                if (_isLevel7Visible == value)
                    return;
                _isLevel7Visible = value;
                RaisePropertyChanged(nameof(IsLevel7Visible));
            }
        }

        public bool IsLevel8Visible
        {
            get => _isLevel8Visible;
            set
            {
                if (_isLevel8Visible == value)
                    return;
                _isLevel8Visible = value;
                RaisePropertyChanged(nameof(IsLevel8Visible));
            }
        }

        public bool IsLevel9Visible
        {
            get => _isLevel9Visible;
            set
            {
                if (_isLevel9Visible == value)
                    return;
                _isLevel9Visible = value;
                RaisePropertyChanged(nameof(IsLevel9Visible));
            }
        }
    }
}
